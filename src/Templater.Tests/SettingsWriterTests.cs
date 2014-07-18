using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;

namespace Templater.Tests
{
	[TestFixture]
	public class SettingsWriterTests
	{
		[Test]
		public void It_writes_files()
		{
			var files = MockRepository.GenerateStub<IFileWrapper>();
			var writer = new SettingsWriter(files);
			var settings = new Settings
				{
					Environments = new List<Environment>
						{
							new Environment
								{
									Name = "uat",
									FileName = "web.uat.config",
								}
						}
				};

			files.Stub(x => x.ReadAllText(@"C:\web.template.config")).Return(string.Empty);
			writer.Write(@"C:\web.template.config", settings);

			files.AssertWasCalled(x => x.WriteAllText(@"C:\web.uat.config", string.Empty));
		}
	}
}
