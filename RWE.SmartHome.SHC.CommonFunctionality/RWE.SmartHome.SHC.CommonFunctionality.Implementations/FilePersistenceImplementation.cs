using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using RWE.SmartHome.SHC.CommonFunctionality.Interfaces;

namespace RWE.SmartHome.SHC.CommonFunctionality.Implementations;

public class FilePersistenceImplementation : IFilePersistence
{
	public class PersistenceRoot
	{
		[XmlAttribute]
		public FactoryResetRequestedStatus FactoryResetStatus { get; set; }

		[XmlAttribute]
		public string TimeZoneName { get; set; }

		[XmlAttribute]
		public int ElevatedLogLevelTimeRemaining { get; set; }

		[XmlAttribute]
		public bool EnableSerialLogging { get; set; }

		public PendingUpdateTasksStatus PendingUpdateTasks { get; set; }

		public UpdatePerformedStatus UpdatePerformedStatus { get; set; }

		public List<string> ElevatedLogLevelModules { get; set; }

		public List<Guid> ActiveDevices { get; set; }

		[XmlAttribute]
		public bool InteractionFunctionValuesFixed { get; set; }

		public LocalUser LocalUser { get; set; }

		public bool LocalAccessEnabled { get; set; }

		public bool InteractionsVerfied { get; set; }

		public bool DevicesKeysExported { get; set; }

		public string EmailReminderSendingTime { get; set; }

		public EmailSettings EmailSettings { get; set; }

		public string ApplicationsTokenHash { get; set; }
	}

	private const string PersistenceFile = "\\NandFlash\\localStorage";

	private PersistenceRoot storage = new PersistenceRoot();

	private readonly XmlSerializer serializer = new XmlSerializer(typeof(PersistenceRoot));

	public FactoryResetRequestedStatus FactoryResetStatus
	{
		get
		{
			return storage.FactoryResetStatus;
		}
		set
		{
			storage.FactoryResetStatus = value;
			SavePersistedFile();
		}
	}

	public string TimeZoneName
	{
		get
		{
			return storage.TimeZoneName;
		}
		set
		{
			storage.TimeZoneName = value;
			SavePersistedFile();
		}
	}

	public int ElevatedLogLevelTimeRemaining
	{
		get
		{
			return storage.ElevatedLogLevelTimeRemaining;
		}
		set
		{
			storage.ElevatedLogLevelTimeRemaining = value;
			SavePersistedFile();
		}
	}

	public bool EnableSerialLogging
	{
		get
		{
			return storage.EnableSerialLogging;
		}
		set
		{
			storage.EnableSerialLogging = value;
			SavePersistedFile();
		}
	}

	public bool InteractionFunctionValuesFixed
	{
		get
		{
			return storage.InteractionFunctionValuesFixed;
		}
		set
		{
			storage.InteractionFunctionValuesFixed = value;
			SavePersistedFile();
		}
	}

	public PendingUpdateTasksStatus PendingUpdateTasks
	{
		get
		{
			return storage.PendingUpdateTasks;
		}
		set
		{
			storage.PendingUpdateTasks = value;
			SavePersistedFile();
		}
	}

	public UpdatePerformedStatus UpdatePerformedStatus
	{
		get
		{
			return storage.UpdatePerformedStatus;
		}
		set
		{
			storage.UpdatePerformedStatus = value;
			SavePersistedFile();
		}
	}

	public List<string> ElevatedLogLevelModules
	{
		get
		{
			if (storage.ElevatedLogLevelModules == null)
			{
				storage.ElevatedLogLevelModules = new List<string>();
			}
			return storage.ElevatedLogLevelModules;
		}
		set
		{
			storage.ElevatedLogLevelModules = value;
			SavePersistedFile();
		}
	}

	public List<Guid> ActiveDevices
	{
		get
		{
			if (storage.ActiveDevices == null)
			{
				storage.ActiveDevices = new List<Guid>();
			}
			return storage.ActiveDevices;
		}
		set
		{
			storage.ActiveDevices = value;
			SavePersistedFile();
		}
	}

	public LocalUser LocalUser
	{
		get
		{
			return storage.LocalUser;
		}
		set
		{
			storage.LocalUser = value;
			SavePersistedFile();
		}
	}

	public bool LocalAccessEnabled
	{
		get
		{
			return storage.LocalAccessEnabled;
		}
		set
		{
			storage.LocalAccessEnabled = value;
			SavePersistedFile();
		}
	}

	public bool InteractionsVerfied
	{
		get
		{
			return storage.InteractionsVerfied;
		}
		set
		{
			storage.InteractionsVerfied = value;
			SavePersistedFile();
		}
	}

	public bool DevicesKeysExported
	{
		get
		{
			return storage.DevicesKeysExported;
		}
		set
		{
			storage.DevicesKeysExported = value;
			SavePersistedFile();
		}
	}

	public string EmailReminderSendingTime
	{
		get
		{
			return storage.EmailReminderSendingTime;
		}
		set
		{
			storage.EmailReminderSendingTime = value;
			SavePersistedFile();
		}
	}

	public EmailSettings EmailSettings
	{
		get
		{
			return storage.EmailSettings;
		}
		set
		{
			storage.EmailSettings = value;
			SavePersistedFile();
		}
	}

	public string ApplicationsTokenHash
	{
		get
		{
			return storage.ApplicationsTokenHash;
		}
		set
		{
			storage.ApplicationsTokenHash = value;
			SavePersistedFile();
		}
	}

	public FilePersistenceImplementation()
	{
		if (File.Exists("\\NandFlash\\localStorage"))
		{
			LoadPersistedFile();
		}
	}

	private void LoadPersistedFile()
	{
		bool flag = false;
		using (FileStream stream = new FileStream("\\NandFlash\\localStorage", FileMode.Open, FileAccess.Read, FileShare.Read))
		{
			try
			{
				storage = (PersistenceRoot)serializer.Deserialize(stream);
			}
			catch (Exception ex)
			{
				flag = true;
				Console.WriteLine("Failed to load local persistence file. " + ex.ToString());
			}
		}
		if (flag)
		{
			File.Delete("\\NandFlash\\localStorage");
			if (storage == null)
			{
				storage = new PersistenceRoot();
			}
			SavePersistedFile();
		}
	}

	private void SavePersistedFile()
	{
		using FileStream stream = new FileStream("\\NandFlash\\localStorage", FileMode.Create, FileAccess.Write, FileShare.Write);
		serializer.Serialize(stream, storage);
	}
}
