using System.Collections.Generic;

namespace Unleash.Client
{
	public class FakeUnleash : IUnleash
	{
		private bool _enableAll;
		private bool _disableAll;
		private readonly IDictionary<string, bool> _features;

		public FakeUnleash()
		{
			_enableAll = false;
			_disableAll = false;
			_features = new Dictionary<string, bool>();
		}

		public bool IsEnabled(string toggleName)
		{
			return IsEnabled(toggleName, false);
		}

		public bool IsEnabled(string toggleName, bool defaultSetting)
		{
			if (_enableAll)
			{
				return true;
			}
			if (_disableAll)
			{
				return false;
			}
			return _features.TryGetValue(toggleName, out var featureEnabled) ? featureEnabled : defaultSetting;
		}

		public void EnableAll()
		{
			_disableAll = false;
			_enableAll = true;
			_features.Clear();
		}

		public void DisableAll()
		{
			_disableAll = true;
			_enableAll = false;
			_features.Clear();
		}

		public void ResetAll()
		{
			_disableAll = false;
			_enableAll = false;
			_features.Clear();
		}

		public void Enable(params string[] features)
		{
			foreach (var feature in features)
			{
				//TODO: handle that an item with the same key exists
				_features.Add(feature, true);
			}
		}

		public void Disable(params string[] features)
		{
			foreach (var feature in features)
			{
				//TODO: handle that an item with the same key exists
				_features.Add(feature, false);
			}
		}

		public void Reset(params string[] features)
		{
			foreach (var feature in features)
			{
				_features.Remove(feature);
			}
		}
	}
}
