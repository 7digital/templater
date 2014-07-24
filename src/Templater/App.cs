using System;
using CommandLine;

namespace Templater
{
	public class App
	{
		private static void Main(string[] args)
		{
			try
			{
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
					Console.WriteLine("Processing - " + file.TemplatePath);

					var settings = reader.Read(file.SettingsPath);
					writer.Write(file.TemplatePath, globals, settings);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("FAILED - " + e.Message);
			}
		}
	}
}
