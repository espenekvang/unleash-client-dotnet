using System;

namespace Unleash.Client
{
	public class UnleashException : Exception
	{
		public UnleashException(string message, Exception innerException) : base(message, innerException)
		{
			
		}
	}
}
