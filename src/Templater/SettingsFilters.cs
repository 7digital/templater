using System;

namespace Templater
{
	public static class SettingsFilters
	{
		public static Settings ApplySpecificEnvironmentFilter(this Settings settings, string environment)
		{
			if (environment.ToLower() != "all")
			{
				var chosenSetting = settings.Environments.FindAll(x => x.Name == environment.ToLower());
				if (chosenSetting.Count < 1)
				{
					throw new Exception(string.Format("Invalid environment specified: {0}", environment));
				}
				settings.Environments = chosenSetting;
			}
			return settings;
		}
	}
}