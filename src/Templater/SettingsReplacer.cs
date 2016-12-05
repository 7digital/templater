using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using log4net;

namespace Templater
{
	public interface ISettingsReplacer
	{
		string Replace(string input, Environment globals, Environment environment);
	}

	public class SettingsReplacer : ISettingsReplacer
	{
		private readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public HashSet<string> UnusedKeys { get; private set; }

		public string Replace(string input, Environment globals, Environment environment)
		{
			globals = globals ?? new Environment();
			UnusedKeys = new HashSet<string>();

			input = EnvironmentVariableReplacer.SwapEnvironmentVariables(input, environment);

			foreach (var key in environment.Values.Keys.Where(key => !input.ContainsKey(key)).Where(key => !UnusedKeys.Contains(key.ToLower())))
				UnusedKeys.Add(key.ToLower());

			input = input.ReplaceKey("Environment", environment.Name);
			input = environment.Values.Aggregate(input, (text, value) => text.ReplaceKey(value.Key, value.Value));
			input = globals.Values.Aggregate(input, (text, value) => text.ReplaceKey(value.Key, value.Value));

			CheckForMissedKeys(input, environment);
			LogUnusedKeys(environment);

			return input;
		}

		private static void CheckForMissedKeys(string input, Environment environment)
		{
			var missed = Regex.Matches(input, @"\[\%[\w\d]+\%\]").Cast<Match>();

			if (missed.Any())
				throw new SettingsTokensNotReplacedException(environment.Name, missed.Select(x => x.Groups[0].ToString()).Distinct());
		}

		private void LogUnusedKeys(Environment environment)
		{
			if (!UnusedKeys.Any())
				return;

			_log.Warn("Unused Keys:");
			foreach (var key in UnusedKeys)
				_log.WarnFormat("[%{0}%] in '{1}' settings", key, environment.Name);
		}
	}

	public static class StringExtensions
	{
		public static string _pattern = @"\[\%{0}\%\]";

		public static string ReplaceKey(this string text, string key, string value)
		{
			if (string.IsNullOrEmpty(key))
				return text;

			return Regex.Replace(text, string.Format(_pattern, key), value ?? string.Empty, RegexOptions.IgnoreCase);
		}

		public static bool ContainsKey(this string text, string key)
		{
			return Regex.Match(text, string.Format(_pattern, key), RegexOptions.IgnoreCase).Success;
		}
	}
}

