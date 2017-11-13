using System.Collections.Generic;

namespace Unleash.Client
{
	public class UnleashContext
	{
		internal readonly string RemoteAddress;
		public string UserId { get; }
		public string SessionId { get; }
		public IDictionary<string, string> Properties { get; }

		public UnleashContext(string userId, string sessionId, string remoteAddress, IDictionary<string, string> properties)
		{
			UserId = userId;
			SessionId = sessionId;
			RemoteAddress = remoteAddress;
			Properties = properties;
		}

		public static Builder GetBuilder()
		{
			return new Builder();
		}

		public class Builder
		{
			private string _userId;
			private string _sessionId;
			private string _remoteAddress;

			private readonly IDictionary<string, string> _properties = new Dictionary<string, string>();

			public Builder WithUserId(string userId)
			{
				_userId = userId;
				return this;
			}

			public Builder WithSessionId(string sessionId)
			{
				_sessionId = sessionId;
				return this;
			}

			public Builder WithRemoteAddress(string remoteAddress)
			{
				_remoteAddress = remoteAddress;
				return this;
			}

			public Builder AddProperty(string name, string value)
			{
				_properties.Add(name, value);
				return this;
			}

			public UnleashContext Build()
			{
				return new UnleashContext(_userId, _sessionId, _remoteAddress, _properties);
			}
		}
	}
}
