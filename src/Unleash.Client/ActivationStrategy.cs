using System.Collections.Generic;

namespace Unleash.Client
{
	public class ActivationStrategy
	{
		public readonly string Name;
		public IDictionary<string, string> Parameters;

		public ActivationStrategy(string name, IDictionary<string, string> parameters)
		{
			Name = name;
			Parameters = parameters;
		}
	}
}
