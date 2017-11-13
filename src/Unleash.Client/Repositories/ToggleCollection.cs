using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Unleash.Client.Repositories
{
	public class ToggleCollection
	{
		private IList<FeatureToggle> features;
		private int version = 1;
		private IDictionary<string, FeatureToggle> cache;

		public ToggleCollection(IList<FeatureToggle> features)
		{
			this.features = ensureNotNull(features);
			this.cache = new HashMap<>();
			for (FeatureToggle featureToggle : this.features)
			{
				cache.put(featureToggle.getName(), featureToggle);
			}
		}

		private IList<FeatureToggle> EnsureNotNull(Collection<FeatureToggle> features)
		{
			if (features == null) { return Enumerable.Empty<FeatureToggle>().ToList(); }
			return features;
		}

		public IList<FeatureToggle> GetFeatures()
		{
			return features;
		}

		FeatureToggle GetToggle(string name)
		{
			return cache.get(name);
		}
	}
}
