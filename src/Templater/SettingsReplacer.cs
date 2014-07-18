using System.Linq;
using System.Text.RegularExpressions;

namespace Templater
{
	public class SettingsReplacer
	{
		public string Replace(string input, Environment environment)
		{
			input = Replace(input, "Environment", environment.Name);
			input = environment.Values.Aggregate(input, (current, value) => Replace(current, value.Key, value.Value));

			var matches = Regex.Matches(input, @"\[\%[\w\d]+\%\]").Cast<Match>();

			if (matches.Any())
				throw new SettingsMissingException(environment.Name, matches.Select(x => x.Groups[0].ToString()).Distinct());

			return input;
		}

		private string Replace(string text, string key, string value)
		{
			if (string.IsNullOrEmpty(key))
				return text;
				 
			return Regex.Replace(text, @"\[\%" + key + @"\%\]", value?? string.Empty, RegexOptions.IgnoreCase);
		}
	}
}
