using System;
using System.Collections.Generic;
using System.Text;
using Unleash.Client.Utils;

namespace Unleash.Client.Metrics
{
	public class UnleashMetricsSender
	{
		//TODO: Add logging
		//private static final Logger LOG = LogManager.getLogger();
		private static int CONNECT_TIMEOUT = 1000;

		private UnleashConfig unleashConfig;
		private Uri clientRegistrationURL;
		private Uri clientMetricsURL;

		public UnleashMetricsSender(UnleashConfig unleashConfig)
		{
			this.unleashConfig = unleashConfig;
			UnleashUrls urls = unleashConfig.UnleashUrLs;
			this.clientMetricsURL = urls.ClientMetricsUrl;
			this.clientRegistrationURL = urls.ClientRegisterUrl;
		}


		public void registerClient(ClientRegistration registration)
		{
			if (!unleashConfig.DisableMetrics)
			{
				try
				{
					post(clientRegistrationURL, registration);
				}
				catch (UnleashException ex)
				{
					LOG.warn("failed to register client", ex);
				}
			}
		}
	}

	public void sendMetrics(ClientMetrics metrics)
{
	if (!unleashConfig.isDisableMetrics())
	{
		try
		{
			post(clientMetricsURL, metrics);
		}
		catch (UnleashException ex)
		{
			LOG.warn("failed to send metrics", ex);
		}
	}
}

private int post(URL url, Object o) throws UnleashException
{

	HttpURLConnection connection = null;
        try {
		connection = (HttpURLConnection)url.openConnection();
		connection.setConnectTimeout(CONNECT_TIMEOUT);
		connection.setReadTimeout(CONNECT_TIMEOUT);
		connection.setRequestMethod("POST");
		connection.setRequestProperty("Accept", "application/json");
		connection.setRequestProperty("Content-Type", "application/json");
		UnleashConfig.setRequestProperties(connection, this.unleashConfig);
		connection.setUseCaches(false);
		connection.setDoInput(true);
		connection.setDoOutput(true);

		OutputStreamWriter wr = new OutputStreamWriter(connection.getOutputStream());
		gson.toJson(o, wr);
		wr.flush();
		wr.close();

		connection.connect();

		return connection.getResponseCode();
	} catch (IOException e) {
		throw new UnleashException("Could not post to Unleash API", e);
	} catch (IllegalStateException e) {
		throw new UnleashException(e.getMessage(), e);
	} finally {
		if (connection != null)
		{
			connection.disconnect();
		}
	}
}
}
}
