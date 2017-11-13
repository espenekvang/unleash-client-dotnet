namespace Unleash.Client.Repositories
{
	public interface IToggleRepository
	{
		FeatureToggle GetToggle(string name);
	}
}
