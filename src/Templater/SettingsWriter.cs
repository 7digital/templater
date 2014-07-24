using System;
using System.IO;
using System.Linq;

namespace Templater
{
	public class SettingsWriter
	{
		private readonly IFileWrapper _files;
		private readonly ISettingsReplacer _replacer;

		public SettingsWriter(IFileWrapper files, ISettingsReplacer replacer)
		{
			_files = files;
			_replacer = replacer;
		}

		public void Write(string path, Settings globals, Settings settings)
		{
			var text = _files.ReadAllText(path);

			foreach (var environment in settings.Environments)
			{
				try
				{
					var globalEnv = globals.Environments.SingleOrDefault(x => x.Name == environment.Name);
					var output = _replacer.Replace(text, globalEnv, environment);
					
					var filename = path.Replace(Path.GetFileName(path), environment.FileName);
					_files.WriteAllText(filename, output);
				}
				catch (SettingsTokensNotReplacedException e)
				{
					throw new Exception(string.Format("File {0} has missing tokens for Environment {1}\n{2}", path, e.Environment, string.Join("\n", e.Keys)));
				}
			}
		}
	}
}
