using CommandLine;
using CommandLine.Text;

namespace Templater
{
	public class Options
	{
		[Option('d', "directory", Required = true, HelpText = "The working directory to recursively scan")]
		public string Directory { get; set; }

		[Option('g', "global-settings", Required = false, HelpText = "Path to a global settings json file")]
		public string GlobalSettingsPath { get; set; }

		[Option('e', "environment", Required = false, HelpText = "Run for a specific environment only - defaults to all", DefaultValue="all")]
		public string RunEnvironment { get; set; }

		[HelpOption]
		public string Help()
		{
			return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
		}
	}
}