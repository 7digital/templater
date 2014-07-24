using System;
using System.Collections.Generic;

namespace Templater
{
	public class SettingsTokensNotReplacedException : Exception
	{
		public string Environment { get; set; }
		public IEnumerable<string> Keys { get; private set; }

		public SettingsTokensNotReplacedException(string environment, IEnumerable<string> keys)
		{
			Environment = environment;
			Keys = keys;
		}
	}
}
