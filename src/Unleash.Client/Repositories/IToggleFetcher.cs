namespace Unleash.Client.Repositories
{
	public interface IToggleFetcher
	{
		FeatureToggleResponse FetchToggles();
	}
}
