using System.Collections.Generic;

namespace Unleash.Client.Metrics
{
	public interface IUnleashMetricService
	{
		void Register(HashSet<string> strategies);
		void Count(string toggleName, bool active);
	}
}
