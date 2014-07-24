using System.Linq;
using System.Text.RegularExpressions;

namespace Templater
{
	public interface ISettingsReplacer
	{
		string Replace(string input, Environment globals, Environment environment);
	}

	public class SettingsReplacer : ISettingsReplacer
	{
		public string Replace(string input, Environment globals, Environment environment)
		{
			input = ReplaceKeyWithValue(input, "Environment", environment.Name);
			input = environment.Values.Aggregate(input, (current, value) => ReplaceKeyWithValue(current, value.Key, value.Value));

			if(globals != null)
				input = globals.Values.Aggregate(input, (current, value) => ReplaceKeyWithValue(current, value.Key, value.Value));

			var matches = Regex.Matches(input, @"\[\%[\w\d]+\%\]").Cast<Match>();

			if (matches.Any())
				throw new SettingsTokensNotReplacedException(environment.Name, matches.Select(x => x.Groups[0].ToString()).Distinct());

			return input;
		}

		private string ReplaceKeyWithValue(string text, string key, string value)
		{
			if (string.IsNullOrEmpty(key))
				return text;
				 
			return Regex.Replace(text, @"\[\%" + key + @"\%\]", value?? string.Empty, RegexOptions.IgnoreCase);
		}
	}
}
