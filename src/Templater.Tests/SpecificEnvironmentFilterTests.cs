using NUnit.Framework;
using System;

namespace Templater.Tests
{
	public class SpecificEnvironmentFilterTests
	{
		private Settings _settings;

		[SetUp]
		public void Given_a_settings_file_containing_environments()
		{
			_settings = new Settings()
			{
				Environments = new System.Collections.Generic.List<Environment>()
				{
					new Environment() {Name = "local"},
					new Environment() {Name = "uat"},
					new Environment() {Name = "prod"}
				}
			};
		}

		[Test]
		public void When_filter_specified_then_should_only_contain_correct_environment()
		{
			var filteredSettings = _settings.ApplySpecificEnvironmentFilter("local");
			Assert.That(filteredSettings.Environments.Count, Is.EqualTo(1));
			Assert.That(filteredSettings.Environments[0].Name, Is.EqualTo("local"));
		}

		[Test]
		public void When_all_specified_should_contain_all_environment()
		{
			var filteredSettings = _settings.ApplySpecificEnvironmentFilter("all");
			Assert.That(filteredSettings.Environments.Count, Is.EqualTo(3));
		}

		[Test]
		public void When_invalid_environment_specified_then_throws_error()
		{
			var exception = Assert.Throws<Exception>(() => _settings.ApplySpecificEnvironmentFilter("deepspace"));
			Assert.That(exception.Message, Is.EqualTo("Invalid environment specified: deepspace"));
		}
	}
}