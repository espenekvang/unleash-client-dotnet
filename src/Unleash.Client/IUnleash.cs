namespace Unleash.Client
{
	public interface IUnleash
	{
		bool IsEnabled(string toggleName);
		bool IsEnabled(string toggleName, bool defaultSetting);

		/*
		  default boolean isEnabled(String toggleName, UnleashContext context) {
        return isEnabled(toggleName, context, false);
    }

    default boolean isEnabled(String toggleName, UnleashContext context, boolean defaultSetting) {
        return isEnabled(toggleName, defaultSetting);
    }
		 */
	}
}
