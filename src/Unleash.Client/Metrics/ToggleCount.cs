using System;
using System.Collections.Generic;
using System.Text;

namespace Unleash.Client.Metrics
{
	internal class ToggleCount
	{
		public long Yes { get; private set; }
		public long No { get; private set; }

		public ToggleCount()
		{
			Yes = 0;
			No = 0;
		}

		public void Register(bool active)
		{
			if (active)
			{
				Yes++;
			}
			else
			{
				No++;
			}
		}
	}
}
