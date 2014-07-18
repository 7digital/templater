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
				var writer = new SettingsWriter(fileWrapper);

				var files = fileWrapper.Find(options.Directory);

				Console.WriteLine("Found files:");
				foreach (var file in files)
				{
					Console.WriteLine(file);
				}
				Console.WriteLine();

				foreach (var file in files)
				{
					Console.WriteLine("Processing - " + file);

					var settings = reader.Read(file);
					writer.Write(file, settings);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("FAILED - " + e.Message);
			}
		}
	}
}
