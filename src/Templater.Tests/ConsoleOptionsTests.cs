using NUnit.Framework;
using System;
using CommandLine;

namespace Templater.Tests
{
	public class ConsoleOptionsTests
	{
		private Options _options;

		[SetUp]
		public void Given_all_args_specified()
		{
			_options = new Options();
			var args = new[] {"-d", "src", "-g", "settings.json", "-e", "local"};
			Parser.Default.ParseArguments(args, _options);
		}

		[Test]
		public void the_help_text_is_valid()
		{
			var helptext = _options.Help();
			Console.WriteLine(helptext);
			Assert.That(helptext, Is.StringContaining("-d, --directory"));
			Assert.That(helptext, Is.StringContaining("Required. The working directory to recursively scan"));
			Assert.That(helptext, Is.StringContaining("-g, --global-settings"));
			Assert.That(helptext, Is.StringContaining("Path to a global settings json file"));
			Assert.That(helptext, Is.StringContaining("-e, --environment"));
			Assert.That(helptext, Is.StringContaining("(Default: all) Run for a specific environment only"));
		}

		[Test]
		public void the_directory_is_specified()
		{
			Assert.That(_options.Directory, Is.EqualTo("src"));
		}

		[Test]
		public void the_globalsettings_param_is_specified()
		{
			Assert.That(_options.GlobalSettingsPath, Is.EqualTo("settings.json"));
		}

		[Test]
		public void the_environment_param_is_specified()
		{
			Assert.That(_options.RunEnvironment, Is.EqualTo("local"));
		}
	}
}
