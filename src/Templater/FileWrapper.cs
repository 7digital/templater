using System.Collections.Generic;
using System.IO;
using System.Linq;
using log4net;

namespace Templater
{
	public interface IFileWrapper
	{
		bool Exists(string path);
		string ReadAllText(string path);
		void WriteAllText(string path, string contents);
		IEnumerable<File> Find(string root, string pattern);
	}

	public class FileWrapper : IFileWrapper
	{
		private readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public bool Exists(string path)
		{
			return System.IO.File.Exists(path);
		}

		public string ReadAllText(string path)
		{
			return System.IO.File.ReadAllText(path);
		}

		public void WriteAllText(string path, string contents)
		{
			System.IO.File.WriteAllText(path, contents);
		}

		public IEnumerable<File> Find(string root, string pattern="*.template.*")
		{
			if (!Directory.Exists(root))
				throw new DirectoryNotFoundException(root);

			var files =  Directory.GetFiles(root, pattern, SearchOption.AllDirectories).ToList();

			_log.Info("Found files:");
			foreach (var file in files) { _log.Info(file); }

			return files.Select(x => new File(x));
		}
	}
}
