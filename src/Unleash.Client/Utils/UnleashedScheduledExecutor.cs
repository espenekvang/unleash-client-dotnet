﻿namespace Unleash.Client.Utils
{
	public class UnleashScheduledExecutor : IUnleashScheduledExecutor
	{

	/*private static final Logger LOG = LogManager.getLogger();

	private final ScheduledThreadPoolExecutor timer;

	public UnleashScheduledExecutorImpl()
	{
		this.timer = new ScheduledThreadPoolExecutor(
			1,
			r-> {
			Thread thread = Executors.defaultThreadFactory().newThread(r);
			thread.setName("unleash-api-executor");
			thread.setDaemon(true);
			return thread;
		});

		this.timer.setRemoveOnCancelPolicy(true);
	}

	@Override
	public ScheduledFuture setInterval(Runnable command,
		long initialDelaySec,
		long periodSec)
	{
		try
		{
			return timer.scheduleAtFixedRate(command, initialDelaySec, periodSec, TimeUnit.SECONDS);
		}
		catch (RejectedExecutionException ex)
		{
			LOG.error("Unleash background task crashed", ex);
			return null;
		}

	}*/
	}
}
