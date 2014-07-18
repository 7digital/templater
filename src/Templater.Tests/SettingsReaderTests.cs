using System.IO;
using System.Linq;
using NUnit.Framework;
using Newtonsoft.Json;
using Rhino.Mocks;

namespace Templater.Tests
{
	[TestFixture]
	public class SettingsReaderTests
	{
		private SettingsReader _reader;
		private IFileWrapper _files;

		[SetUp]
		public void TestFixtureSetUp()
		{
			_files = MockRepository.GenerateStub<IFileWrapper>();
			_reader = new SettingsReader(_files);
		}

		[Test]
		public void It_throws_an_exception_if_there_is_not_a_settings_file()
		{
			_files.Stub(x => x.Exists(@"C:\doesnotexist.settings.json")).Return(false);

			Assert.Throws<FileNotFoundException>(() => _reader.Read(@"C:\doesnotexist.template.config"));
		}

		[Test]
		public void It_throws_an_exception_if_the_settings_is_invalid()
		{
			_files.Stub(x => x.Exists(Arg<string>.Is.Anything)).Return(true);
			_files.Stub(x => x.ReadAllText(Arg<string>.Is.Anything))
				.Return("{\"environments\":[{\"name\":\"uat\",\"values\":{\"debug\":\"false\",\"stacktrace\":\"false\"");

			Assert.Throws<JsonSerializationException>(() => _reader.Read(@"C:\invalid.template.config"));
		}

		[Test]
		public void It_reads_settings()
		{
			_files.Stub(x => x.Exists(Arg<string>.Is.Anything)).Return(true);
			_files.Stub(x => x.ReadAllText(Arg<string>.Is.Anything))
				.Return("{\"environments\":[{\"name\":\"uat\",\"values\":{\"debug\":\"false\",\"stacktrace\":\"false\"}}]}");

			var settings = _reader.Read(@"C:\valid.template.config");

			Assert.That(settings.Environments.Count, Is.EqualTo(1));
			Assert.That(settings.Environments[0].Name, Is.EqualTo("uat"));
			Assert.That(settings.Environments[0].Values.Count, Is.EqualTo(2));
			Assert.That(settings.Environments[0].Values.First().Key, Is.EqualTo("debug"));
			Assert.That(settings.Environments[0].Values.First().Value, Is.EqualTo("false"));
		}

		[Test]
		public void It_reads_empty_values()
		{
			_files.Stub(x => x.Exists(Arg<string>.Is.Anything)).Return(true);
			_files.Stub(x => x.ReadAllText(Arg<string>.Is.Anything))
				.Return("{\"environments\":[{\"name\":\"uat\",\"values\":{\"debug\":\"\",\"stacktrace\":\"\"}}]}");

			var settings = _reader.Read(@"C:\valid.template.config");

			Assert.That(settings.Environments[0].Values.Count, Is.EqualTo(2));
			Assert.That(settings.Environments[0].Values.First().Key, Is.EqualTo("debug"));
			Assert.That(settings.Environments[0].Values.First().Value, Is.EqualTo(string.Empty));
		}
	}
}
