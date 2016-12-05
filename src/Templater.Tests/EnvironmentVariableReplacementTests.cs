using System.Linq;
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

		[SetUp]
		public void Given_enviroment_variables_exist()
		{
			System.Environment.SetEnvironmentVariable("SomeKey_LOCAL", "I replaced SomeKey");
			System.Environment.SetEnvironmentVariable("AnotherKey_LOCAL", "I replaced AnotherKey");
		}

		[Test]
		public void It_can_get_list_of_tokens_from_input_text()
		{
			var actual = EnvironmentVariableReplacer.GetTokens(INPUT_TEXT).ToArray();

			Assert.That(actual.Length, Is.EqualTo(3));
			Assert.That(actual[0], Is.EqualTo("SomeKey"));
			Assert.That(actual[1], Is.EqualTo("AnotherKey"));
			Assert.That(actual[2], Is.EqualTo("AnUnmatchedKey"));
		}

		[Test]
		public void It_can_swap_out_enviroment_variables_based_on_list_of_tokens()
		{
			var actual = EnvironmentVariableReplacer.SwapEnvironmentVariables(INPUT_TEXT, new Environment() { Name="local" });

			var expected = "Example I replaced SomeKey \r\n" +
				"Another example I replaced AnotherKey \r\n" +
				"Another example [%AnUnmatchedKey%] \r\n" +
				"You can't match this [%NoMatch";

			Assert.That(actual, Is.EqualTo(expected));
		}

		[TearDown]
		public void Remove_enviroment_variables()
		{
			System.Environment.SetEnvironmentVariable("SomeKey_LOCAL", null);
			System.Environment.SetEnvironmentVariable("AnotherKey_LOCAL", null);
		}
	}
}