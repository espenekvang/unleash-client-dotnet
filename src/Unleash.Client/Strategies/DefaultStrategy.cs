using System.Collections.Generic;

namespace Unleash.Client.Strategies
{
	public class DefaultStrategy : IStrategy
	{
		private const string StrategyName = "default";

		public string GetName()
		{
			return StrategyName;
		}

		public bool IsEnabled(IDictionary<string, string> parameters)
		{
			return true;
		}
	}
}
