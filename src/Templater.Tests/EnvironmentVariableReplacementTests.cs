using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Templater.Tests
{
	[TestFixture]
	public class EnvironmentVariableReplacementTests
	{
		[Test]
		public void It_can_get_list_of_tokens_from_input_text()
		{
			var inputText = "Example [%SomeKey%] \r\n" +
				"Another example [%AnotherKey%] \r\n" +
				"Another example [%AnUnmatchedKey%] \r\n" +
				"You can't match this [%NoMatch";

			var actual = GetTokens(inputText).ToArray();

			Assert.That(actual.Length, Is.EqualTo(3));
			Assert.That(actual[0], Is.EqualTo("SomeKey"));
			Assert.That(actual[1], Is.EqualTo("AnotherKey"));
			Assert.That(actual[2], Is.EqualTo("AnUnmatchedKey"));
		}

		private IEnumerable<string> GetTokens(string input)
		{
			var matches = Regex.Matches(input, @"\[\%(.+?)\%\]")
				.Cast<Match>();
			return matches.Select(x=>x.Groups[1].Value);
		}
	}
}