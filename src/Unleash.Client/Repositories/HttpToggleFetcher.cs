using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Unleash.Client.Utils;

namespace Unleash.Client.Repositories
{
	public class HttpToggleFetcher : IToggleFetcher
	{
		private const int ConnectTimeout = 10000;
		private string _etag = string.Empty;

		private readonly Uri _toggleUrl;
		private readonly UnleashConfig _unleashConfig;

		public HttpToggleFetcher(UnleashConfig unleashConfig)
		{
			_unleashConfig = unleashConfig;
			_toggleUrl = unleashConfig.UnleashUrLs.FetchTogglesUrl;
		}

		public FeatureToggleResponse FetchToggles()
		{
			try
			{
				using (var client = new HttpClient())
				{
					var request = new HttpRequestMessage(HttpMethod.Get, _toggleUrl);
					request.Content.Headers.Add("Content-Type", "application/json");

					client.Timeout = new TimeSpan(0, 0, 0, 0, ConnectTimeout);
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					client.DefaultRequestHeaders.Add("If-None-Match", _etag);
					UnleashConfig.SetRequestProperties(client, _unleashConfig);

					//TODO: async/await needed
					var response = client.SendAsync(request).GetAwaiter().GetResult();

					return response.IsSuccessStatusCode
						? GetToggleResponse(response)
						: new FeatureToggleResponse(FeatureToggleStatus.NotChanged);
				}
			}
			catch (InvalidOperationException e)
			{
				throw new UnleashException(e.Message, e);
			}
		}

		private FeatureToggleResponse GetToggleResponse(HttpResponseMessage response)
		{
			var headers = response.Headers;

			if (headers.TryGetValues("ETag", out var values))
			{
				_etag = values.FirstOrDefault() ?? string.Empty;
			}

			//TODO: async/await needed
			var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
			var toggleCollection = JsonToggleParser.FromJson(content);

			return new FeatureToggleResponse(FeatureToggleStatus.Changed, toggleCollection);
		}
	}
}
