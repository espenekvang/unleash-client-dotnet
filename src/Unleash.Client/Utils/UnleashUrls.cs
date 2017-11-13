using System;

namespace Unleash.Client.Utils
{
    public class UnleashUrls
    {
	    public Uri FetchTogglesUrl { get; }
	    public Uri ClientMetricsUrl { get; }
	    public Uri ClientRegisterUrl { get; }

	    public UnleashUrls(Uri unleashApi)
	    {
		    FetchTogglesUrl = new Uri(unleashApi, "feaures");
			ClientMetricsUrl = new Uri(unleashApi, "client/metrices");
			ClientRegisterUrl = new Uri(unleashApi, "client/register");
	    }
	}
}
