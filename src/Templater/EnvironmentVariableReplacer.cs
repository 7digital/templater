using log4net;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Templater
{
	public static class EnvironmentVariableReplacer
	{
		public static string SwapEnvironmentVariables(string input, Environment environment)
		{
			foreach (var token in GetTokens(input))
			{
				var environmentVariable = System.Environment.GetEnvironmentVariable(FormEnvVarKey(token, environment));
				if (!string.IsNullOrEmpty(environmentVariable))
				{
					input = input.ReplaceKey(token, environmentVariable);
				}
			}

			return input;
		}

		public static IEnumerable<string> GetTokens(string input)
		{
			var matches = Regex.Matches(input, @"\[\%(.+?)\%\]")
				.Cast<Match>();
			return matches.Select(x=>x.Groups[1].Value);
		}

		private static string FormEnvVarKey(string token, Environment environment)
		{
			if (environment != null && !string.IsNullOrEmpty(environment.Name))
			{
				return string.Format("{0}_{1}", token, environment.Name);
			}

			return token;
		}
	}
}