using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Templater.Tests
{
	[TestFixture]
	public class EnvironmentVariableReplacementTests
	{
		private const string INPUT_TEXT = "Example [%SomeKey%] \r\n" +
			"Another example [%AnotherKey%] \r\n" +
			"Another example [%AnUnmatchedKey%] \r\n" +
			"You can't match this [%NoMatch";

		[Test]
		public void It_can_get_list_of_tokens_from_input_text()
		{
			var actual = GetTokens(INPUT_TEXT).ToArray();

			Assert.That(actual.Length, Is.EqualTo(3));
			Assert.That(actual[0], Is.EqualTo("SomeKey"));
			Assert.That(actual[1], Is.EqualTo("AnotherKey"));
			Assert.That(actual[2], Is.EqualTo("AnUnmatchedKey"));
		}

		[Test]
		public void It_can_swap_out_enviroment_variables_based_on_list_of_tokens()
		{
			System.Environment.SetEnvironmentVariable("SomeKey", "I replaced SomeKey");
			System.Environment.SetEnvironmentVariable("AnotherKey", "I replaced AnotherKey");

			var actual = SwapEnvironmentVariables(INPUT_TEXT);

			var expected = "Example I replaced SomeKey \r\n" +
				"Another example I replaced AnotherKey \r\n" +
				"Another example [%AnUnmatchedKey%] \r\n" +
				"You can't match this [%NoMatch";

			Assert.That(actual, Is.EqualTo(expected));
		}

		private object SwapEnvironmentVariables(string input)
		{
			foreach (var token in GetTokens(input))
			{
				var environmentVariable = System.Environment.GetEnvironmentVariable(token);
				if (!string.IsNullOrEmpty(environmentVariable))
				{
					input = input.ReplaceKey(token, environmentVariable);
				}
			}

			return input;
		}

		private IEnumerable<string> GetTokens(string input)
		{
			var matches = Regex.Matches(input, @"\[\%(.+?)\%\]")
				.Cast<Match>();
			return matches.Select(x=>x.Groups[1].Value);
		}
	}
}