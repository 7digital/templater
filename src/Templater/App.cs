using System;
using CommandLine;
using log4net;
using log4net.Config;

namespace Templater
{
	public class App
	{
		private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private static void Main(string[] args)
		{
			try
			{
				XmlConfigurator.Configure();

				var options = new Options();

				if (!Parser.Default.ParseArguments(args, options))
					return;

				var fileWrapper = new FileWrapper();
				var reader = new SettingsReader(fileWrapper);
				var writer = new SettingsWriter(fileWrapper, new SettingsReplacer());
				
				var files = fileWrapper.Find(options.Directory);
				var globals = new Settings();
				
				if(!string.IsNullOrEmpty(options.GlobalSettingsPath))
					globals = reader.Read(options.GlobalSettingsPath);

				foreach (var file in files)
				{
					_log.InfoFormat("Processing - {0}", file.TemplatePath);

					var settings = reader.Read(file.SettingsPath);
					writer.Write(file.TemplatePath, globals, settings);
				}
			}
			catch (Exception e)
			{
				_log.Error(e);
			}
		}
	}
}
