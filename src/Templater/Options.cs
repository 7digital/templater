using CommandLine;
using CommandLine.Text;

namespace Templater
{
	public class Options
	{
		[Option('d', "directory", Required = true, HelpText = "The working directory to recursively scan")]
		public string Directory { get; set; }

		[HelpOption]
		public string Help()
		{
			return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
		}
	}
}