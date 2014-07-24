using System.IO;
using Newtonsoft.Json;
using log4net;

namespace Templater
{
	public class SettingsReader
	{
		private readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		private readonly IFileWrapper _files;

		public SettingsReader(IFileWrapper files)
		{
			_files = files;
		}

		public Settings Read(string path)
		{
			if(!_files.Exists(path))
				throw new FileNotFoundException("Settings file not found - " + path);

			_log.InfoFormat("Reading settings - {0}", path);
			var json = _files.ReadAllText(path);
			
			return JsonConvert.DeserializeObject<Settings>(json);
		}
	}
}
