using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;

namespace Templater.Tests
{
	[TestFixture]
	public class SettingsWriterTests
	{
		[Test]
		public void It_writes_for_each_environment()
		{
			var files = MockRepository.GenerateStub<IFileWrapper>();
			var replacer = MockRepository.GenerateStub<ISettingsReplacer>();

			var writer = new SettingsWriter(files, replacer);
			var settings = new Settings
				{
					Environments = new List<Environment>
						{
							new Environment
								{
									Name = "uat",
									FileName = "web.uat.config",
								},
							new Environment
								{
									Name = "prod",
									FileName = "web.prod.config",
								},
						}
				};

			writer.Write(@"C:\web.template.config", new Settings(), settings);

			files.AssertWasCalled(x => x.WriteAllText(@"C:\web.uat.config", null));
			files.AssertWasCalled(x => x.WriteAllText(@"C:\web.prod.config", null));
		}

		[Test]
		public void It_writes_for_specific_environment_if_specified()
		{
			var files = MockRepository.GenerateStub<IFileWrapper>();
			var replacer = MockRepository.GenerateStub<ISettingsReplacer>();

			var writer = new SettingsWriter(files, replacer);
			var settings = new Settings
				{
					Environments = new List<Environment>
						{
							new Environment
								{
									Name = "uat",
									FileName = "web.uat.config",
								},
							new Environment
								{
									Name = "prod",
									FileName = "web.prod.config",
								},
						}
				};

			writer.Write(@"C:\web.template.config", new Settings(), settings);

			files.AssertWasCalled(x => x.WriteAllText(@"C:\web.uat.config", null));
			files.AssertWasCalled(x => x.WriteAllText(@"C:\web.prod.config", null));
		}
	}
}
