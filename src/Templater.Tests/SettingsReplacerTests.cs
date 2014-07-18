using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Templater.Tests
{
	[TestFixture]
	public class SettingsReplacerTests
	{
		private SettingsReplacer _replacer;

		[SetUp]
		public void SetUp()
		{
			_replacer = new SettingsReplacer();
		}

		[Test]
		public void It_replaces_environment_name()
		{
			var output = _replacer.Replace("Example [%Environment%]", new Environment {Name = "uat"});
			Assert.That(output, Is.EqualTo("Example uat"));
		}

		[Test]
		public void It_replaces_values()
		{
			var output = _replacer.Replace("Example [%SomeKey%] [%Another%]", new Environment { Values = new Dictionary<string, string> { { "SomeKey", "foo" }, { "another", "bar" } } });
			Assert.That(output, Is.EqualTo("Example foo bar"));
		}

		[Test]
		public void It_is_case_insensitive_for_keys()
		{
			var output = _replacer.Replace("Example [%SomeKey%] [%Another%]", new Environment { Values = new Dictionary<string, string> { { "somekey", "foo" }, {"another", "bar"} } });
			Assert.That(output, Is.EqualTo("Example foo bar"));			
		}

		[Test]
		public void It_replaces_emtpy_values()
		{
			var output = _replacer.Replace("Example [%SomeKey%]", new Environment { Values = new Dictionary<string, string> { { "somekey", "" } } });
			Assert.That(output, Is.EqualTo("Example "));
		}

		[Test]
		public void It_throws_an_exception_if_there_are_still_remaining_tokens()
		{
			try
			{
				_replacer.Replace("Example [%SomeKey%] and [%Another%] [%Another%]", new Environment { Name = "local" });
			}
			catch (SettingsMissingException e)
			{
				Assert.That(e.Environment, Is.EqualTo("local"));
				Assert.That(e.Keys.Count(), Is.EqualTo(2));
				Assert.That(e.Keys.First(), Is.EqualTo("[%SomeKey%]"));
				Assert.That(e.Keys.Last(), Is.EqualTo("[%Another%]"));
				return;
			}

			Assert.Fail();
		}
	}
}
