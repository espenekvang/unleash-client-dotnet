using System.Collections.Generic;

namespace Unleash.Client.Strategies
{
	public interface IStrategy
	{
		string GetName();

		bool IsEnabled(IDictionary<string, string> parameters);
/*
			default boolean isEnabled(Map<String, String> parameters, UnleashContext unleashContext)
		{
			return isEnabled(parameters);
		}*/
	}
}
