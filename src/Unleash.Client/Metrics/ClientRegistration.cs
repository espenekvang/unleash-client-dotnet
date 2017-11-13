using System;
using System.Collections.Generic;
using Unleash.Client.Utils;

namespace Unleash.Client.Metrics
{
	public class ClientRegistration
	{
		public string AppName { get; }
		public string InstanceId { get; }
		public string SdkVersion { get; }
		public HashSet<string> Strategies{ get; }
		public DateTimeOffset Started { get; }
		public long Interval { get; }

		public ClientRegistration(UnleashConfig config, DateTimeOffset started, HashSet<string> strategies)
		{
			AppName = config.AppName;
			InstanceId = config.InstanceId;
			SdkVersion = config.SdkVersion;
			Started = started;
			Strategies = strategies;
			Interval = config.SendMetricsInterval;
		}
	}
}
