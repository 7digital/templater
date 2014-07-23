using System.IO;

namespace Templater
{
	public class File
	{
		public File(string path)
		{
			TemplatePath = path.ToLower();
		}

		public string TemplatePath { get; private set; }

		public string SettingsPath
		{
			get
			{
				return TemplatePath
					.Replace(".template", ".settings")
					.Replace(Path.GetExtension(TemplatePath), ".json");
			}
		}
	}
}
