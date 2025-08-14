using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.Linq;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.Common.GlobalContracts;
using RWE.SmartHome.SHC.Core.Authentication.Entities;
using RWE.SmartHome.SHC.Core.Authentication.Properties;
using RWE.SmartHome.SHC.Core.Database;

namespace RWE.SmartHome.SHC.Core.Authentication;

internal sealed class AuthenticationPersistence : IAuthenticationDataAccess, IService
{
	private readonly DatabaseConnectionsPool persistence;

	private User internalUser;

	public Guid InternalUserId => internalUser.Id;

	public AuthenticationPersistence(Container container)
	{
		persistence = container.Resolve<DatabaseConnectionsPool>();
	}

	private User CreateInternalUser(DatabaseConnection connection)
	{
		IDictionary<Guid, User> users = GetUsers();
		Guid guid = Guid.NewGuid();
		while (users.ContainsKey(guid))
		{
			guid = Guid.NewGuid();
		}
		User user = new User();
		user.Id = guid;
		user.Name = "SHC";
		user.LastLoginDate = DateTime.UtcNow;
		user.Roles = new List<Role>
		{
			new Role
			{
				Id = DefaultRoles.Shc.Id,
				Name = DefaultRoles.Shc.Name
			}
		};
		return user;
	}

	public DatabaseConnection OpenDatabaseTransaction()
	{
		DatabaseConnection connection = persistence.GetConnection();
		connection.BeginTransaction();
		return connection;
	}

	public IDictionary<Guid, User> GetUsers()
	{
		Dictionary<Guid, User> dictionary = new Dictionary<Guid, User>();
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand("Users");
		databaseCommand.CommandType = CommandType.TableDirect;
		databaseCommand.IndexName = "Users_PKey";
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)2);
		try
		{
			IEnumerable<User> source = from SqlCeUpdatableRecord record in (IEnumerable)val
				select new User
				{
					Id = record.GetGuid(record.GetOrdinal("Id")),
					Name = record.GetString(record.GetOrdinal("Name")),
					Password = record.GetString(record.GetOrdinal("Password")),
					CreateDate = record.GetDateTime(record.GetOrdinal("CreateDate")),
					LastLoginDate = record.GetDateTime(record.GetOrdinal("LastLoginDate")),
					Roles = GetRolesByUser(record.GetGuid(record.GetOrdinal("Id"))).ToList()
				};
			return source.ToDictionary((User x) => x.Id);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public void CreateUser(User user)
	{
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand("Users");
		databaseCommand.CommandType = CommandType.TableDirect;
		databaseCommand.IndexName = "Users_PKey";
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)3);
		try
		{
			SqlCeUpdatableRecord val2 = val.CreateRecord();
			val2.SetGuid(val2.GetOrdinal("Id"), user.Id);
			val2.SetString(val2.GetOrdinal("Name"), user.Name);
			val2.SetString(val2.GetOrdinal("Password"), user.Password);
			val2.SetDateTime(val2.GetOrdinal("CreateDate"), user.CreateDate);
			val2.SetDateTime(val2.GetOrdinal("LastLoginDate"), user.LastLoginDate);
			val.Insert(val2);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public User ReadUser(Guid userId)
	{
		if (internalUser != null && userId == internalUser.Id)
		{
			return internalUser;
		}
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand("Users");
		databaseCommand.CommandType = CommandType.TableDirect;
		databaseCommand.IndexName = "Users_PKey";
		databaseCommand.SetRange((DbRangeOptions)16, new object[1] { userId }, null);
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)2);
		try
		{
			if (((DbDataReader)(object)val).Read())
			{
				User user = new User();
				user.Id = ((DbDataReader)(object)val).GetGuid(((DbDataReader)(object)val).GetOrdinal("Id"));
				user.Name = ((DbDataReader)(object)val).GetString(((DbDataReader)(object)val).GetOrdinal("Name"));
				user.Password = ((DbDataReader)(object)val).GetString(((DbDataReader)(object)val).GetOrdinal("Password"));
				user.CreateDate = ((DbDataReader)(object)val).GetDateTime(((DbDataReader)(object)val).GetOrdinal("CreateDate"));
				user.LastLoginDate = ((DbDataReader)(object)val).GetDateTime(((DbDataReader)(object)val).GetOrdinal("LastLoginDate"));
				user.Roles = GetRolesByUser(((DbDataReader)(object)val).GetGuid(((DbDataReader)(object)val).GetOrdinal("Id"))).ToList();
				return user;
			}
			return null;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public User ReadUser(string userName)
	{
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(Resources.SelectUserByName);
		databaseCommand.Parameters.Add("UserName", (object)userName);
		SqlCeResultSet resultSet = databaseCommand.ExecuteResultSet((ResultSetOptions)2);
		try
		{
			IEnumerable<User> source = from IDataRecord record in (IEnumerable)resultSet
				select new User
				{
					Id = ((DbDataReader)(object)resultSet).GetGuid(((DbDataReader)(object)resultSet).GetOrdinal("Id")),
					Name = ((DbDataReader)(object)resultSet).GetString(((DbDataReader)(object)resultSet).GetOrdinal("Name")),
					Password = ((DbDataReader)(object)resultSet).GetString(((DbDataReader)(object)resultSet).GetOrdinal("Password")),
					CreateDate = ((DbDataReader)(object)resultSet).GetDateTime(((DbDataReader)(object)resultSet).GetOrdinal("CreateDate")),
					LastLoginDate = ((DbDataReader)(object)resultSet).GetDateTime(((DbDataReader)(object)resultSet).GetOrdinal("LastLoginDate")),
					Roles = GetRolesByUser(((DbDataReader)(object)resultSet).GetGuid(((DbDataReader)(object)resultSet).GetOrdinal("Id"))).ToList()
				};
			return source.DefaultIfEmpty(null).FirstOrDefault();
		}
		finally
		{
			if (resultSet != null)
			{
				((IDisposable)resultSet).Dispose();
			}
		}
	}

	public void UpdateUser(User user)
	{
		if (internalUser != null && user.Id == internalUser.Id)
		{
			return;
		}
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand("Users");
		databaseCommand.CommandType = CommandType.TableDirect;
		databaseCommand.IndexName = "Users_PKey";
		databaseCommand.SetRange((DbRangeOptions)16, new object[1] { user.Id }, null);
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)3);
		try
		{
			if (((DbDataReader)(object)val).Read())
			{
				val.SetString(((DbDataReader)(object)val).GetOrdinal("Name"), user.Name);
				val.SetString(((DbDataReader)(object)val).GetOrdinal("Password"), user.Password);
				val.SetDateTime(((DbDataReader)(object)val).GetOrdinal("CreateDate"), user.CreateDate);
				val.SetDateTime(((DbDataReader)(object)val).GetOrdinal("LastLoginDate"), user.LastLoginDate);
				val.Update();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public void DeleteUser(User user)
	{
		if (internalUser != null && user.Id == internalUser.Id)
		{
			return;
		}
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand("Users");
		databaseCommand.CommandType = CommandType.TableDirect;
		databaseCommand.IndexName = "Users_PKey";
		databaseCommand.SetRange((DbRangeOptions)16, new object[1] { user.Id }, null);
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

	public IDictionary<Guid, Role> GetRoles()
	{
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand("Roles");
		databaseCommand.CommandType = CommandType.TableDirect;
		databaseCommand.IndexName = "Roles_PKey";
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)2);
		try
		{
			IEnumerable<Role> source = from SqlCeUpdatableRecord record in (IEnumerable)val
				select new Role
				{
					Id = record.GetGuid(record.GetOrdinal("Id")),
					Name = record.GetString(record.GetOrdinal("Name"))
				};
			return source.ToDictionary((Role x) => x.Id);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public void CreateRole(Role role)
	{
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand("Roles");
		databaseCommand.CommandType = CommandType.TableDirect;
		databaseCommand.IndexName = "Roles_PKey";
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)3);
		try
		{
			SqlCeUpdatableRecord val2 = val.CreateRecord();
			val2.SetGuid(val2.GetOrdinal("Id"), role.Id);
			val2.SetString(val2.GetOrdinal("Name"), role.Name);
			val.Insert(val2);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public Role ReadRole(Guid roleId)
	{
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand("Roles");
		databaseCommand.CommandType = CommandType.TableDirect;
		databaseCommand.IndexName = "Roles_PKey";
		databaseCommand.SetRange((DbRangeOptions)16, new object[1] { roleId }, null);
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)2);
		try
		{
			if (((DbDataReader)(object)val).Read())
			{
				Role role = new Role();
				role.Id = ((DbDataReader)(object)val).GetGuid(((DbDataReader)(object)val).GetOrdinal("Id"));
				role.Name = ((DbDataReader)(object)val).GetString(((DbDataReader)(object)val).GetOrdinal("Name"));
				return role;
			}
			return null;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public void UpdateRole(Role role)
	{
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand("Roles");
		databaseCommand.CommandType = CommandType.TableDirect;
		databaseCommand.IndexName = "Roles_PKey";
		databaseCommand.SetRange((DbRangeOptions)16, new object[1] { role.Id }, null);
		SqlCeResultSet val = databaseCommand.ExecuteResultSet((ResultSetOptions)3);
		try
		{
			if (((DbDataReader)(object)val).Read())
			{
				val.SetString(((DbDataReader)(object)val).GetOrdinal("Name"), role.Name);
				val.Update();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public void DeleteRole(Role role)
	{
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand("Roles");
		databaseCommand.CommandType = CommandType.TableDirect;
		databaseCommand.IndexName = "Roles_PKey";
		databaseCommand.SetRange((DbRangeOptions)16, new object[1] { role.Id }, null);
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

	public IList<Role> GetRolesByUser(Guid userId)
	{
		if (internalUser != null && userId == internalUser.Id)
		{
			return internalUser.Roles;
		}
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(Resources.SelectRolesByUser);
		databaseCommand.Parameters.Add("UserId", (object)userId);
		SqlCeResultSet resultSet = databaseCommand.ExecuteResultSet((ResultSetOptions)2);
		try
		{
			IEnumerable<Role> source = from IDataRecord record in (IEnumerable)resultSet
				select new Role
				{
					Id = ((DbDataReader)(object)resultSet).GetGuid(((DbDataReader)(object)resultSet).GetOrdinal("Id")),
					Name = ((DbDataReader)(object)resultSet).GetString(((DbDataReader)(object)resultSet).GetOrdinal("Name"))
				};
			return source.ToList();
		}
		finally
		{
			if (resultSet != null)
			{
				((IDisposable)resultSet).Dispose();
			}
		}
	}

	public IList<User> GetUsersByRole(Guid roleId)
	{
		List<User> list;
		using (DatabaseConnection databaseConnection = persistence.GetConnection())
		{
			using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(Resources.SelectUsersByRole);
			databaseCommand.Parameters.Add("RoleId", (object)roleId);
			SqlCeResultSet resultSet = databaseCommand.ExecuteResultSet((ResultSetOptions)2);
			try
			{
				IEnumerable<User> source = from IDataRecord record in (IEnumerable)resultSet
					select new User
					{
						Id = ((DbDataReader)(object)resultSet).GetGuid(((DbDataReader)(object)resultSet).GetOrdinal("Id")),
						Name = ((DbDataReader)(object)resultSet).GetString(((DbDataReader)(object)resultSet).GetOrdinal("Name")),
						Password = ((DbDataReader)(object)resultSet).GetString(((DbDataReader)(object)resultSet).GetOrdinal("Password")),
						CreateDate = ((DbDataReader)(object)resultSet).GetDateTime(((DbDataReader)(object)resultSet).GetOrdinal("CreateDate")),
						LastLoginDate = ((DbDataReader)(object)resultSet).GetDateTime(((DbDataReader)(object)resultSet).GetOrdinal("LastLoginDate")),
						Roles = GetRolesByUser(((DbDataReader)(object)resultSet).GetGuid(((DbDataReader)(object)resultSet).GetOrdinal("Id"))).ToList()
					};
				list = source.ToList();
			}
			finally
			{
				if (resultSet != null)
				{
					((IDisposable)resultSet).Dispose();
				}
			}
		}
		if (internalUser != null && internalUser.Roles.Find((Role role) => role.Id == roleId) != null)
		{
			list.Add(internalUser);
		}
		return list;
	}

	public void AddUserToRole(User user, Role role)
	{
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(Resources.AddUserToRole);
		databaseCommand.Parameters.Add("UserId", (object)user.Id);
		databaseCommand.Parameters.Add("RoleId", (object)role.Id);
		databaseCommand.ExecuteNonQuery();
		user.Roles = GetRolesByUser(user.Id).ToList();
	}

	public void RemoveUserFromRole(User user, Role role)
	{
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateDatabaseCommand(Resources.RemoveUserFromRole);
		databaseCommand.Parameters.Add("UserId", (object)user.Id);
		databaseCommand.Parameters.Add("RoleId", (object)role.Id);
		databaseCommand.ExecuteNonQuery();
		user.Roles = GetRolesByUser(user.Id).ToList();
	}

	public void Initialize()
	{
		List<string> list = new List<string>();
		list.Add(Resources.Users);
		list.Add(Resources.Users_PKey);
		list.Add(Resources.Roles);
		list.Add(Resources.Roles_PKey);
		list.Add(Resources.UsersRoles);
		list.Add(Resources.UsersRoles_PKey);
		list.Add(Resources.UsersRoles_Users_FKey);
		list.Add(Resources.UsersRoles_Roles_FKey);
		List<string> list2 = list;
		using DatabaseConnection databaseConnection = persistence.GetConnection();
		foreach (string item in list2)
		{
			databaseConnection.ExecuteNonQuery(item);
		}
		internalUser = CreateInternalUser(databaseConnection);
	}

	public void Uninitialize()
	{
	}
}
