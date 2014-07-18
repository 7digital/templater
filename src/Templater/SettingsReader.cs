using System;
using System.IO;
using Newtonsoft.Json;

namespace Templater
{
	public class SettingsReader
	{
		private readonly IFileWrapper _files;

		public SettingsReader(IFileWrapper files)
		{
			_files = files;
		}

		public Settings Read(string path)
		{
			path = path.ToLower();
			path = path.Replace(".template", ".settings");
			path = path.Replace(Path.GetExtension(path), ".json");

			if(!_files.Exists(path))
				throw new FileNotFoundException("Settings file not found - " + path);

			Console.WriteLine("Reading settings - " + path);
			var json = _files.ReadAllText(path);
			
			return JsonConvert.DeserializeObject<Settings>(json);
		}
	}
}
