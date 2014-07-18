using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Templater
{
	public interface IFileWrapper
	{
		bool Exists(string path);
		string ReadAllText(string path);
		void WriteAllText(string path, string contents);
		List<string> Find(string root, string pattern);
	}

	public class FileWrapper : IFileWrapper
	{
		public bool Exists(string path)
		{
			return File.Exists(path);
		}

		public string ReadAllText(string path)
		{
			return File.ReadAllText(path);
		}

		public void WriteAllText(string path, string contents)
		{
			File.WriteAllText(path, contents);
		}

		public List<string> Find(string root, string pattern="*.template.*")
		{
			if (!Directory.Exists(root))
				throw new DirectoryNotFoundException(root);

			return Directory.GetFiles(root, pattern, SearchOption.AllDirectories).ToList();
		}
	}
}
