using System;
using System.Collections.Generic;

namespace Templater
{
	public class SettingsMissingException : Exception
	{
		public string Environment { get; set; }
		public IEnumerable<string> Keys { get; private set; }

		public SettingsMissingException(string environment, IEnumerable<string> keys)
		{
			Environment = environment;
			Keys = keys;
		}
	}
}
