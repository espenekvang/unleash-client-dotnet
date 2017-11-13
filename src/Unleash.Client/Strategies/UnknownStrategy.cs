using System.Collections.Generic;

namespace Unleash.Client.Strategies
{
	public class UnknownStrategy : IStrategy
	{
		private const string StrategyName = "unknown";

		public string GetName()
		{
			return StrategyName;
		}

		public bool IsEnabled(IDictionary<string, string> parameters)
		{
			return false;
		}
	}
}
