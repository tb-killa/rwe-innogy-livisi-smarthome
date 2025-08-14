using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.Linq;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Database;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DataAccess.Properties;
using RWE.SmartHome.SHC.DataAccessInterfaces.Messages;

namespace RWE.SmartHome.SHC.DataAccess.Messages;

internal sealed class MessagesAndAlertsPersistence : IMessagePersistence, IService
{
	public const string MESSAGE_ID_COLUMN = "Id";

	public const string MESSAGE_CLASS_COLUMN = "Class";

	public const string MESSAGE_TYPE_COLUMN = "Type";

	public const string MESSAGE_TIMESTAMP_COLUMN = "TimeStamp";

	public const string MESSAGE_STATE_COLUMN = "State";

	public const string MESSAGE_APPID_COLUMN = "AppId";

	public const string MESSAGE_ADDINVERSION_COLUMN = "AddInVersion";

	public const string MESSAGE_BASEDEVICEIDS_COLUMN = "DeviceIds";

	public const string MESSAGE_LOGICALDEVICEIDS_COLUMN = "CapabilityIds";

	public const string TABLE = "Messages";

	public const string PRIMARY_KEY = "Messages_PKey";

	private readonly DatabaseConnectionsPool persistence;

	private Dictionary<Guid, Message> messagesCache;

	private readonly object cacheLock = new object();

	public MessagesAndAlertsPersistence(Container container)
	{
		persistence = container.Resolve<DatabaseConnectionsPool>();
	}

	public void UpdateState(Guid messageId, MessageState newState)
	{
		ExecuteInTransaction(delegate
		{
			UpdateMessageState(messageId, newState);
		});
		lock (cacheLock)
		{
			messagesCache.TryGetValue(messageId, out var value);
			if (value != null)
			{
				value.State = newState;
			}
		}
	}

	private void UpdateMessageState(Guid messageId, MessageState newState)
	{
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand("Messages");
		databaseCommand.CommandType = CommandType.TableDirect;
		databaseCommand.IndexName = "Messages_PKey";
		databaseCommand.SetRange((DbRangeOptions)16, new object[1] { messageId }, null);
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)3);
		try
		{
			if (((DbDataReader)(object)val).Read())
			{
				val.SetByte(((DbDataReader)(object)val).GetOrdinal("State"), (byte)newState);
				val.Update();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public void Create(Message message)
	{
		ExecuteInTransaction(delegate
		{
			CreateMessage(message);
		});
		lock (cacheLock)
		{
			messagesCache.Add(message.Id, message);
		}
	}

	private void CreateMessage(Message message)
	{
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand("Messages");
		databaseCommand.CommandType = CommandType.TableDirect;
		databaseCommand.IndexName = "Messages_PKey";
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)3);
		try
		{
			SqlCeUpdatableRecord val2 = val.CreateRecord();
			val2.SetGuid(val2.GetOrdinal("Id"), message.Id);
			val2.SetByte(val2.GetOrdinal("Class"), (byte)message.Class);
			val2.SetString(val2.GetOrdinal("Type"), message.Type);
			val2.SetDateTime(val2.GetOrdinal("TimeStamp"), message.TimeStamp);
			val2.SetByte(val2.GetOrdinal("State"), (byte)message.State);
			val2.SetString(val2.GetOrdinal("AppId"), message.AppId ?? string.Empty);
			val2.SetString(val2.GetOrdinal("AddInVersion"), message.AddinVersion ?? string.Empty);
			val2.SetString(val2.GetOrdinal("DeviceIds"), ToString(message.BaseDeviceIds));
			val2.SetString(val2.GetOrdinal("CapabilityIds"), ToString(message.LogicalDeviceIds));
			val.Insert(val2);
			foreach (StringProperty item in message.Properties.Where((StringProperty mp) => mp.Value != null))
			{
				CreateMessageParameter(message.Id, item);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private string ToString(List<Guid> deviceIds)
	{
		if (deviceIds != null && deviceIds.Count > 0)
		{
			return string.Join(",", deviceIds.Select((Guid id) => id.ToString("N")).ToArray());
		}
		return string.Empty;
	}

	private void CreateMessageParameter(Guid messageId, StringProperty messageParameter)
	{
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand("MessageParameters");
		databaseCommand.CommandType = CommandType.TableDirect;
		databaseCommand.IndexName = "MessagesParameters_PKey";
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)3);
		try
		{
			SqlCeUpdatableRecord val2 = val.CreateRecord();
			val2.SetGuid(val2.GetOrdinal("Id"), Guid.NewGuid());
			val2.SetGuid(val2.GetOrdinal("MessageId"), messageId);
			val2.SetString(val2.GetOrdinal("Key"), messageParameter.Name);
			val2.SetString(val2.GetOrdinal("Value"), messageParameter.Value);
			val.Insert(val2);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private List<StringProperty> GetMessageParameters(Guid messageId)
	{
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(Resources.SelectMessageParametersByMessage);
		databaseCommand.Parameters.Add("MessageId", (object)messageId);
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)2);
		try
		{
			IEnumerable<StringProperty> source = from SqlCeUpdatableRecord record in (IEnumerable)val
				select new StringProperty
				{
					Name = record.GetString(record.GetOrdinal("Key")),
					Value = record.GetString(record.GetOrdinal("Value"))
				};
			return source.ToList();
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public List<Message> GetAll(MessageClass messageClass, string messageType)
	{
		lock (cacheLock)
		{
			return (from m in messagesCache.Values
				where m.Class == messageClass && m.Type == messageType
				select m.Clone()).ToList();
		}
	}

	public List<Message> GetAll()
	{
		lock (cacheLock)
		{
			return messagesCache.Values.Select((Message m) => m.Clone()).ToList();
		}
	}

	private List<Message> GetAllFromDatabase()
	{
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(Resources.SelectMessages);
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)2);
		try
		{
			return ProcessQueryResult(val);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private List<Message> ProcessQueryResult(SqlCeResultSet resultSet)
	{
		List<Message> result = new List<Message>();
		try
		{
			IEnumerable<Message> source = from SqlCeUpdatableRecord record in (IEnumerable)resultSet
				select new Message((MessageClass)record.GetByte(record.GetOrdinal("Class")), record.GetString(record.GetOrdinal("Type")), GetMessageParameters(record.GetGuid(record.GetOrdinal("Id"))))
				{
					Id = record.GetGuid(record.GetOrdinal("Id")),
					TimeStamp = record.GetDateTime(record.GetOrdinal("TimeStamp")),
					State = (MessageState)record.GetByte(record.GetOrdinal("State")),
					AppId = record.GetString(record.GetOrdinal("AppId")),
					AddinVersion = record.GetString(record.GetOrdinal("AddInVersion")),
					BaseDeviceIds = FromString(record.GetString(record.GetOrdinal("DeviceIds"))),
					LogicalDeviceIds = FromString(record.GetString(record.GetOrdinal("CapabilityIds")))
				};
			result = source.ToList();
		}
		catch (Exception ex)
		{
			Log.Error(Module.DataAccess, "Error processing query result:" + ex.Message);
		}
		return result;
	}

	private List<Guid> FromString(string list)
	{
		if (!string.IsNullOrEmpty(list))
		{
			return (from id in list.Split(',')
				select new Guid(id)).ToList();
		}
		return new List<Guid>();
	}

	public void Delete(Guid messageId)
	{
		ExecuteInTransaction(delegate
		{
			DeleteMessage(messageId);
		});
		lock (cacheLock)
		{
			messagesCache.Remove(messageId);
		}
	}

	private void DeleteMessage(Guid messageId)
	{
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand("Messages");
		databaseCommand.CommandType = CommandType.TableDirect;
		databaseCommand.IndexName = "Messages_PKey";
		databaseCommand.SetRange((DbRangeOptions)16, new object[1] { messageId }, null);
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)3);
		try
		{
			if (((DbDataReader)(object)val).Read())
			{
				val.Delete();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public List<Message> GetParamValue(MessageParameterKey param, string value)
	{
		lock (cacheLock)
		{
			return messagesCache.Values.Where((Message m) => m.Properties.Any((StringProperty p) => p.Name == param.ToString() && p.Value == value)).ToList();
		}
	}

	public Message Get(Guid id)
	{
		lock (cacheLock)
		{
			messagesCache.TryGetValue(id, out var value);
			return value;
		}
	}

	public DatabaseConnection OpenDatabaseTransaction()
	{
		DatabaseConnection connection = persistence.GetConnection();
		connection.BeginTransaction();
		return connection;
	}

	private void ExecuteInTransaction(Action action)
	{
		using DatabaseConnection databaseConnection = OpenDatabaseTransaction();
		try
		{
			action();
			databaseConnection.CommitTransaction();
		}
		catch (Exception ex)
		{
			Log.Error(Module.DataAccess, string.Concat("Error accessing the DB: ", ex, " stack: ", ex.StackTrace));
			databaseConnection.RollbackTransaction();
			throw;
		}
	}

	public List<MessageInfo> CreateBackup()
	{
		lock (cacheLock)
		{
			return messagesCache.Values.Select((Message message) => new MessageInfo
			{
				Message = message.Clone()
			}).ToList();
		}
	}

	public void RestoreFromBackup(List<MessageInfo> messageInfos)
	{
		ExecuteInTransaction(delegate
		{
			RestoreBackup(messageInfos);
		});
		lock (cacheLock)
		{
			messagesCache = messageInfos.Select((MessageInfo mi) => mi.Message).ToDictionary((Message msg) => msg.Id);
		}
	}

	private void RestoreBackup(List<MessageInfo> messageInfos)
	{
		List<Message> all = GetAll();
		foreach (Message item in all)
		{
			Delete(item.Id);
		}
		ExecuteInTransaction(delegate
		{
			foreach (MessageInfo messageInfo in messageInfos)
			{
				CreateMessage(messageInfo.Message);
			}
		});
	}

	public bool ContainsMessage(Guid id)
	{
		return messagesCache.Any((KeyValuePair<Guid, Message> x) => x.Key == id);
	}

	public bool ContainsMessage(Func<Message, bool> predicate)
	{
		if (predicate == null)
		{
			return true;
		}
		return messagesCache.Values.Any(predicate);
	}

	public void Initialize()
	{
		List<string> list = new List<string>();
		list.Add(Resources.Messages);
		list.Add(Resources.Messages_PKey);
		list.Add(Resources.MessageParameters);
		list.Add(Resources.MessageParameters_PKey);
		list.Add(Resources.MessageParameters_Messages_FKey);
		List<string> commandTexts = list;
		using (DatabaseConnection databaseConnection = persistence.GetConnection())
		{
			databaseConnection.ExecuteNonQueries(commandTexts);
		}
		lock (cacheLock)
		{
			messagesCache = GetAllFromDatabase().ToDictionary((Message msg) => msg.Id);
		}
	}

	public void Uninitialize()
	{
	}
}
