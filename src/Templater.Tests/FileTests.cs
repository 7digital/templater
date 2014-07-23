using NUnit.Framework;

namespace Templater.Tests
{
	[TestFixture]
	public class FileTests
	{
		[Test]
		public void It_returns_paths()
		{
			var file = new File(@"c:\something\example.template.config");

			Assert.That(file.TemplatePath, Is.EqualTo(@"c:\something\example.template.config"));
			Assert.That(file.SettingsPath, Is.EqualTo(@"c:\something\example.settings.json"));
		}
	}
}
