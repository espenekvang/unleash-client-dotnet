using System;
using System.Collections.Generic;
using System.Text;
using Unleash.Client.Utils;

namespace Unleash.Client.Metrics
{
	public class UnleashMetricServiceImpl : IUnleashMetricService
	{

	private DateTimeOffset started;
	private UnleashConfig unleashConfig;
	private long metricsInterval;
	private UnleashMetricsSender unleashMetricsSender;

	//mutable
	private MetricsBucket currentMetricsBucket;

	public UnleashMetricServiceImpl(UnleashConfig unleashConfig, UnleashScheduledExecutor executor)
	{
		this(unleashConfig, new UnleashMetricsSender(unleashConfig), executor);
	}

	public UnleashMetricServiceImpl(UnleashConfig unleashConfig,
		UnleashMetricsSender unleashMetricsSender,
		UnleashScheduledExecutor executor)
	{
		this.currentMetricsBucket = new MetricsBucket();
		this.started = LocalDateTime.now(ZoneId.of("UTC"));
		this.unleashConfig = unleashConfig;
		this.metricsInterval = unleashConfig.getSendMetricsInterval();
		this.unleashMetricsSender = unleashMetricsSender;

		executor.setInterval(sendMetrics(), metricsInterval, metricsInterval);
	}

	@Override
	public void register(Set<String> strategies)
	{
		ClientRegistration registration = new ClientRegistration(unleashConfig, started, strategies);
		unleashMetricsSender.registerClient(registration);
	}

	@Override
	public void count(String toggleName, boolean active)
	{
		currentMetricsBucket.registerCount(toggleName, active);
	}

	private Runnable sendMetrics()
	{
		return ()-> {
			MetricsBucket metricsBucket = this.currentMetricsBucket;
			this.currentMetricsBucket = new MetricsBucket();
			metricsBucket.end();
			ClientMetrics metrics = new ClientMetrics(unleashConfig, metricsBucket);
			unleashMetricsSender.sendMetrics(metrics);
		};
	}
	}
}
