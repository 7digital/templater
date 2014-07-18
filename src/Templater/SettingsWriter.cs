using System;
using System.IO;

namespace Templater
{
	public class SettingsWriter
	{
		private readonly IFileWrapper _files;

		public SettingsWriter(IFileWrapper files)
		{
			_files = files;
		}

		public void Write(string file, Settings settings)
		{
			var text = _files.ReadAllText(file);
			var replacer = new SettingsReplacer();

			foreach (var environment in settings.Environments)
			{
				try
				{
					var output = replacer.Replace(text, environment);
					var filename = file.Replace(Path.GetFileName(file), environment.FileName);
					_files.WriteAllText(filename, output);
				}
				catch (SettingsMissingException e)
				{
					throw new Exception(string.Format("File {0} has missing tokens for Environment {1}\n{2}", file, e.Environment, string.Join("\n", e.Keys)));
				}
			}
		}
	}
}
