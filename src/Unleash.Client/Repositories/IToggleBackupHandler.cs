namespace Unleash.Client.Repositories
{
	public interface IToggleBackupHandler
	{
		ToggleCollection Read();
		void Write(ToggleCollection toggleCollection);
	}
}
