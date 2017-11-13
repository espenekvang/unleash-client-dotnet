using System;
using System.Linq;
using Newtonsoft.Json;

namespace Unleash.Client.Repositories
{
	internal class JsonToggleParser
	{

		private JsonToggleParser()
		{
		}

		public static string ToJsonString(ToggleCollection toggleCollection)
		{
			return JsonConvert.SerializeObject(toggleCollection);
		}


		public static ToggleCollection FromJson(string toggleCollectionJson)
		{
			var toggleCollection = JsonConvert.DeserializeObject<ToggleCollection>(toggleCollectionJson);

			if (toggleCollection == null || !toggleCollection.GetFeatures().Any())
			{
				throw new InvalidOperationException("Could not extract toggles from json");
			}

			return toggleCollection;
		}
	}
}
