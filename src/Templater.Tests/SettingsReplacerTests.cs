using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Templater.Tests
{ 
	[TestFixture]
	public class SettingsReplacerTests
	{
		private SettingsReplacer _replacer;

		[SetUp]
		public void SetUp()
		{
			_replacer = new SettingsReplacer();
		}

		[Test]
		public void It_replaces_environment_name()
		{
			var settings = new Environment {Name = "uat"};
			var output = _replacer.Replace("Example [%Environment%]", null, settings);
			Assert.That(output, Is.EqualTo("Example uat"));
		}

		[Test]
		public void It_replaces_values()
		{
			var settings = new Environment {Values = new Dictionary<string, string> {{"SomeKey", "foo"}, {"another", "bar"}}};
			var output = _replacer.Replace("Example [%SomeKey%] [%Another%]", null, settings);
			Assert.That(output, Is.EqualTo("Example foo bar"));
		}

		[Test]
		public void It_replaces_local_values_before_global_values()
		{
			var settings = new Environment { Values = new Dictionary<string, string> { { "SomeKey", "foo" } } };
			var globals = new Environment { Values = new Dictionary<string, string> { { "SomeKey", "bar" } } };

			var output = _replacer.Replace("Example [%SomeKey%]", globals, settings);
			Assert.That(output, Is.EqualTo("Example foo"));
		}

		[Test]
		public void It_replaces_global_values()
		{
			var globals = new Environment { Values = new Dictionary<string, string> { { "SomeKey", "foo" }, { "another", "bar" } } };
			var output = _replacer.Replace("Example [%SomeKey%] [%Another%]", globals, new Environment());
			Assert.That(output, Is.EqualTo("Example foo bar"));	
		}

		[Test]
		public void It_is_case_insensitive_for_keys()
		{
			var settings = new Environment {Values = new Dictionary<string, string> {{"somekey", "foo"}, {"another", "bar"}}};
			var output = _replacer.Replace("Example [%SomeKey%] [%Another%]", null, settings);
			Assert.That(output, Is.EqualTo("Example foo bar"));			
		}

		[Test]
		public void It_replaces_emtpy_values()
		{
			var settings = new Environment {Values = new Dictionary<string, string> {{"somekey", ""}}};
			var output = _replacer.Replace("Example [%SomeKey%]", null, settings);
			Assert.That(output, Is.EqualTo("Example "));
		}

		[Test]
		public void It_tracks_unused_keys()
		{
			var settings = new Environment { Values = new Dictionary<string, string> { { "SomeKey", "foo" }, { "another", "bar" } } };
			_replacer.Replace("Example [%SomeKey%]", null, settings);
			Assert.That(_replacer.UnusedKeys.Count, Is.EqualTo(1));
			Assert.That(_replacer.UnusedKeys.First(), Is.EqualTo("another"));
		}

		[Test]
		public void It_throws_an_exception_if_there_are_still_remaining_tokens()
		{
			try
			{
				_replacer.Replace("Example [%SomeKey%] and [%Another%] [%Another%]", null, new Environment { Name = "local" });
			}
			catch (SettingsTokensNotReplacedException e)
			{
				Assert.That(e.Environment, Is.EqualTo("local"));
				Assert.That(e.Keys.Count(), Is.EqualTo(2));
				Assert.That(e.Keys.First(), Is.EqualTo("[%SomeKey%]"));
				Assert.That(e.Keys.Last(), Is.EqualTo("[%Another%]"));
				return;
			}

			Assert.Fail();
		}

		[Test]
		public void It_checks_env_var_first()
		{
			System.Environment.SetEnvironmentVariable("EnvironmentVariableKey_LOCAL", "I am some key");

			var replace = _replacer.Replace("Example [%EnvironmentVariableKey%]", null, new Environment { Name = "local" });

			System.Environment.SetEnvironmentVariable("EnvironmentVariableKey_LOCAL", null);
		}
	}
}
