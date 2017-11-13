using System;
using System.Collections.Generic;
using Unleash.Client.Metrics;
using Unleash.Client.Repositories;
using Unleash.Client.Strategies;
using Unleash.Client.Utils;

namespace Unleash.Client
{
	public class DefaultUnleash : IUnleash
	{
		private static IList<IStrategy> _builtInStrategies = new List<IStrategy> {new DefaultStrategy()};
		private static UnknownStrategy UnknownStrategy = new UnknownStrategy();
		private static IUnleashScheduledExecutor unleashScheduledExecutor = new UnleashScheduledExecutor();
		private IUnleashMetricService metricService;
		private IToggleRepository toggleRepository;
		private IDictionary<string, IStrategy> strategyMap;
		private IUnleashContextProvider contextProvider;


		private static FeatureToggleRepository defaultToggleRepository(UnleashConfig unleashConfig)
		{
			return new FeatureToggleRepository(
					unleashConfig,
					unleashScheduledExecutor,
					new HttpToggleFetcher(unleashConfig),
					new ToggleBackupHandlerFile(unleashConfig));
		}

		public DefaultUnleash(UnleashConfig unleashConfig, params IStrategy[] strategies):this(unleashConfig, defaultToggleRepository(unleashConfig), strategies)
		{
		}

		public DefaultUnleash(UnleashConfig unleashConfig, IToggleRepository toggleRepository, params IStrategy[] strategies)
		{
			this.toggleRepository = toggleRepository;
			this.strategyMap = buildStrategyMap(strategies);
			this.contextProvider = unleashConfig.ContextProvider;
			this.metricService = new UnleashMetricServiceImpl(unleashConfig, unleashScheduledExecutor);
			metricService.register(strategyMap.keySet());
		}

		public bool IsEnabled(string toggleName)
		{
			return IsEnabled(toggleName, false);
		}

		
		public bool isEnabled(string toggleName, bool defaultSetting)
		{
			return isEnabled(toggleName, contextProvider.getContext(), defaultSetting);
		}

		public bool IsEnabled(string toggleName, UnleashContext context , bool defaultSetting)
		{
			bool enabled;
			FeatureToggle featureToggle = toggleRepository.getToggle(toggleName);

			if (featureToggle == null)
			{
				enabled = defaultSetting;
			}
			else if (!featureToggle.Enabled)
			{
				enabled = false;
			}
			else
			{
				enabled = featureToggle.Strategies.stream()
						.filter(as -> getStrategy(as.getName()).isEnabled(as.getParameters(), context))
						.findFirst()
						.isPresent();
			}

			count(toggleName, enabled);
			return enabled;
		}

		public FeatureToggle getFeatureToggleDefinition(string toggleName)
		{
			return Optional.ofNullable(toggleRepository.getToggle(toggleName));
		}

		public void count(final String toggleName, boolean enabled)
		{
			metricService.count(toggleName, enabled);
		}

		private IDictionary<string, IStrategy> buildStrategyMap(IStrategy[] strategies)
		{
			IDictionary<string, IStrategy> map = new Dictionary<string, IStrategy>();

			BUILTIN_STRATEGIES.forEach(strategy->map.put(strategy.getName(), strategy));

			if (strategies != null)
			{
				for (Strategy strategy : strategies)
				{
					map.put(strategy.getName(), strategy);
				}
			}

			return map;
		}

		private Strategy getStrategy(String strategy)
		{
			return strategyMap.containsKey(strategy) ? strategyMap.get(strategy) : UNKNOWN_STRATEGY;
		}
	}
}
