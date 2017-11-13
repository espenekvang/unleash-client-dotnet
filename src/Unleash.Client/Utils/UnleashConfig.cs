using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;

namespace Unleash.Client.Utils
{
	public class UnleashConfig
	{
		private const string UnleashAppNameHeader = "UNLEASH-APPNAME";
		private const string UnleashInstanceIdHeader = "UNLEASH-INSTANCEID";

		public Uri UnleashApi { get; }
		public UnleashUrls UnleashUrLs { get; }
		public IDictionary<string, string> CustomHttpHeaders { get; }
		public string AppName { get; }
		public string InstanceId { get; }
		public string SdkVersion { get; }
		public string BackupFile { get; }
		public long FetchTogglesInterval { get; }
		public long SendMetricsInterval { get; }
		public bool DisableMetrics { get; }
		public IUnleashContextProvider ContextProvider { get; }

		public UnleashConfig(
			Uri unleashApi,
			IDictionary<string, string> customHttpHeaders,
			string appName,
			string instanceId,
			string sdkVersion,
			string backupFile,
			long fetchTogglesInterval,
			long sendMetricsInterval,
			bool disableMetrics,
			IUnleashContextProvider contextProvider)
		{
			UnleashApi = unleashApi ?? throw new InvalidOperationException("You are required to specify the unleashAPI url");
			CustomHttpHeaders = customHttpHeaders;
			UnleashUrLs = new UnleashUrls(unleashApi);
			AppName = appName ?? throw new InvalidOperationException("You are required to specify the unleash appName");
			InstanceId = instanceId;
			SdkVersion = sdkVersion;
			BackupFile = backupFile;
			FetchTogglesInterval = fetchTogglesInterval;
			SendMetricsInterval = sendMetricsInterval;
			DisableMetrics = disableMetrics;
			ContextProvider = contextProvider;
		}

		public static Builder GetBuilder()
		{
			return new Builder();
		}

		public static void SetRequestProperties(HttpClient client, UnleashConfig config)
		{
			client.DefaultRequestHeaders.Add("User-Agent", config.AppName);
			client.DefaultRequestHeaders.Add(UnleashAppNameHeader, config.AppName);
			client.DefaultRequestHeaders.Add(UnleashInstanceIdHeader, config.InstanceId);
			foreach (var customHttpHeader in config.CustomHttpHeaders)
			{
				client.DefaultRequestHeaders.Add(customHttpHeader.Key, customHttpHeader.Value);
			}
		}

		public class Builder
		{
			private Uri _unleashApi;
			private readonly IDictionary<string, string> _customHttpHeaders = new Dictionary<string, string>();
			private string _appName;
			private string _instanceId = GetDefaultInstanceId();

			public string SdkVersion
			{
				get
				{
					var assembly = System.Reflection.Assembly.GetExecutingAssembly();
					var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
					return fileVersionInfo.FileVersion;
				}
			}

			private string _backupFile;
			private long _fetchTogglesInterval = 10;
			private long _sendMetricsInterval = 60;
			private bool _disableMetrics;
			private IUnleashContextProvider _contextProvider = new DefaultContextProvider();

			public static string GetDefaultInstanceId()
			{
				var hostName = "";
				try
				{
					hostName = Dns.GetHostName() + "-";
				}
				catch (SocketException)
				{
					//TODO: should we log something here?
				}
				return hostName + "generated-" + Math.Round(new Random(Guid.NewGuid().GetHashCode()).Next() * 1000000.0D);
			}

			public Builder WithUnleashApi(Uri unleashApi)
			{
				_unleashApi = unleashApi;
				return this;
			}

			public Builder WithUnleashApi(string unleashApi)
			{
				_unleashApi = new Uri(unleashApi);
				return this;
			}

			public Builder WithCustomHttpHeader(string name, string value)
			{
				_customHttpHeaders.Add(name, value);
				return this;
			}

			public Builder WithAppName(string appName)
			{
				_appName = appName;
				return this;
			}

			public Builder WithInstanceId(string instanceId)
			{
				_instanceId = instanceId;
				return this;
			}

			public Builder WithFetchTogglesInterval(long fetchTogglesInterval)
			{
				_fetchTogglesInterval = fetchTogglesInterval;
				return this;
			}

			public Builder WithSendMetricsInterval(long sendMetricsInterval)
			{
				_sendMetricsInterval = sendMetricsInterval;
				return this;
			}

			public Builder WithDisableMetrics()
			{
				_disableMetrics = true;
				return this;
			}

			public Builder WitBbackupFile(string backupFile)
			{
				_backupFile = backupFile;
				return this;
			}

			public Builder WithUnleashContextProvider(IUnleashContextProvider contextProvider)
			{
				_contextProvider = contextProvider;
				return this;
			}

			private string GetBackupFile()
			{
				if (_backupFile != null)
				{
					return _backupFile;
				}

				var fileName = "unleash-" + _appName + "-repo.json";
				return Path.Combine(Path.GetTempPath(), fileName);
			}

			public UnleashConfig Build()
			{
				return new UnleashConfig(
					_unleashApi,
					_customHttpHeaders,
					_appName,
					_instanceId,
					SdkVersion,
					GetBackupFile(),
					_fetchTogglesInterval,
					_sendMetricsInterval,
					_disableMetrics,
					_contextProvider);
			}
		}
	}
}