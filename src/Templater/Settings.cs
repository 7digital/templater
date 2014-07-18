using System.Collections.Generic;
using Newtonsoft.Json;

namespace Templater
{
	public class Settings
	{
		public Settings()
		{
			Environments = new List<Environment>();
		}

		[JsonProperty("environments")]
		public List<Environment> Environments { get; set; } 
	}

	public class Environment
	{
		public Environment()
		{
			Values = new Dictionary<string, string>();
		}

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("fileName")]
		public string FileName { get; set; }

		[JsonProperty("values")]
		public Dictionary<string, string> Values { get; set; } 
	}
}
