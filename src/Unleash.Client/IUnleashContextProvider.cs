namespace Unleash.Client
{
	public interface IUnleashContextProvider
	{
		UnleashContext GetContext();
	}

	internal class DefaultContextProvider : IUnleashContextProvider
	{
		public UnleashContext GetContext()
		{
			var builder = UnleashContext.GetBuilder();
			return builder.Build();
		}
	}
}
