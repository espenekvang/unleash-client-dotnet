using System.Collections.Generic;

namespace Unleash.Client
{
	public class FeatureToggle
	{
		public readonly string Name;
		public readonly bool Enabled;
		public readonly IList<ActivationStrategy> Strategies;

		public FeatureToggle(string name, bool enabled, IList<ActivationStrategy> strategies)
		{
			Name = name;
			Enabled = enabled;
			Strategies = strategies;
		}

		public override string ToString()
		{
			return "FeatureToggle{" +
			       "name='" + Name + '\'' +
			       ", enabled=" + Enabled +
			       ", strategies='" + Strategies + '\'' +
			       '}';
		}
	}
}
