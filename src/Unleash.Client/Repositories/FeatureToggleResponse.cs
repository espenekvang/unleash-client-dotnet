using System.Linq;

namespace Unleash.Client.Repositories
{
	public class FeatureToggleResponse
	{
		public FeatureToggleStatus Status { get; }
		public ToggleCollection ToggleCollection{ get; }

		public FeatureToggleResponse(FeatureToggleStatus status, ToggleCollection toggleCollection)
		{
			Status = status;
			ToggleCollection = toggleCollection;
		}

		public FeatureToggleResponse(FeatureToggleStatus status)
		{
			Status = status;
			var emptyList = Enumerable.Empty<FeatureToggle>().ToList();
			ToggleCollection = new ToggleCollection(emptyList);
		}
	}

	public enum FeatureToggleStatus { NotChanged, Changed }
}
