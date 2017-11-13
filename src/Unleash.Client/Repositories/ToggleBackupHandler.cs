using System;
using System.IO;
using System.Linq;
using System.Text;
using Unleash.Client.Utils;

namespace Unleash.Client.Repositories
{
	public class ToggleBackupHandlerFile : IToggleBackupHandler
	{
		//TODO: add logging
		//private static final Logger LOG = LogManager.getLogger();
		private readonly string _backupFile;

		public ToggleBackupHandlerFile(UnleashConfig config)
		{
			_backupFile = config.BackupFile;
		}

		public ToggleCollection Read()
		{
			//LOG.info("Unleash will try to load feature toggle states from temporary backup");
			try
			{
				var toggleCollectionJson = File.ReadAllText(_backupFile);
				return JsonToggleParser.FromJson(toggleCollectionJson);
			}
			catch (FileNotFoundException e)
			{
				//LOG.warn(" Unleash could not find the backup-file '" + backupFile + "'. \n" +"This is expected behavior the first time unleash runs in a new environment.");
			}
			catch (Exception e)
			{
				//LOG.warn("Failed to read backup file:'{}'", backupFile, e);
			}

			return new ToggleCollection(Enumerable.Empty<FeatureToggle>().ToList());
		}

		public void Write(ToggleCollection toggleCollection)
		{
			try
			{
				File.WriteAllText(_backupFile, JsonToggleParser.ToJsonString(toggleCollection), Encoding.UTF8);
			}
			catch (Exception e)
			{
				//LOG.warn("Unleash was unable to backup feature toggles to file: {}", backupFile, e);
			}
		}
	}
}