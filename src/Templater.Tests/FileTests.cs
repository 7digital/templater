using System.IO;
using NUnit.Framework;

namespace Templater.Tests
{
	[TestFixture]
    public class FileTests
	{
		private const string path = @"C:\temp\templater";

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			if (Directory.Exists(path))
				Directory.Delete(path, true);

			Directory.CreateDirectory(path);
			File.Create(path + @"\file1.template.config");
			File.Create(path + @"\file2.template.config");

			Directory.CreateDirectory(path + @"\folder");
			File.Create(path + @"\folder\file1.template.config");
			File.Create(path + @"\folder\file2.template.config");

			Directory.CreateDirectory(path + @"\folder\subfolder");
			File.Create(path + @"\folder\subfolder\file1.template.config");
			File.Create(path + @"\folder\subfolder\file2.template.config");
		}

		[Test]
		public void It_throws_an_exception_if_root_does_not_exist()
		{
			Assert.Throws<DirectoryNotFoundException>(() => new FileWrapper().Find(@"C:\thisdoesnotexist"));
		}

		[Test]
		public void It_finds_files()
		{
			var finder = new FileWrapper();
			var files = finder.Find(path);

			Assert.That(files.Count, Is.EqualTo(6));
		}
    }
}
