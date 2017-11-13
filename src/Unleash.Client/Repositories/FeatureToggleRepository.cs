using System;
using Unleash.Client.Utils;

namespace Unleash.Client.Repositories
{
	public class FeatureToggleRepository : IToggleRepository
	{
	//TODO: Add logging
	///private static final Logger LOG = LogManager.getLogger();

	private IToggleBackupHandler toggleBackupHandler;
	private IToggleFetcher toggleFetcher;

	private ToggleCollection toggleCollection;

	public FeatureToggleRepository(
		UnleashConfig unleashConfig,
		IUnleashScheduledExecutor executor,
		IToggleFetcher toggleFetcher,
		IToggleBackupHandler toggleBackupHandler)
	{

		this.toggleBackupHandler = toggleBackupHandler;
		this.toggleFetcher = toggleFetcher;

		toggleCollection = toggleBackupHandler.read();

		executor.setInterval(updateToggles(), 0, unleashConfig.getFetchTogglesInterval());
	}

	private Runnable updateToggles()
	{
		return ()-> {
			try
			{
				FeatureToggleResponse response = toggleFetcher.fetchToggles();
				if (response.getStatus() == FeatureToggleResponse.Status.CHANGED)
				{
					toggleCollection = response.getToggleCollection();
					toggleBackupHandler.write(response.getToggleCollection());
				}
			}
			catch (UnleashException e)
			{
				LOG.warn("Could not refresh feature toggles", e);
			}
		};
	}

	public FeatureToggle GetToggle(string name)
	{
		return toggleCollection.getToggle(name);
	}
	}}
