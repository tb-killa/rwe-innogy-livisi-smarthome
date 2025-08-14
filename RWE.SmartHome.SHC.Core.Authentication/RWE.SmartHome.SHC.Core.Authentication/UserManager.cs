using System;
using System.Collections.Generic;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.Common.GlobalContracts;
using RWE.SmartHome.SHC.Core.Authentication.Entities;
using RWE.SmartHome.SHC.Core.Database;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.Core.Authentication;

internal sealed class UserManager : IUserManager, IService
{
	private const string LoggingSource = "UserManager";

	private readonly Container dependencyContainer;

	private readonly IAuthenticationDataAccess dataAccess;

	private readonly IEventManager eventManager;

	public ICollection<User> Users
	{
		get
		{
			try
			{
				return dataAccess.GetUsers().Values;
			}
			catch
			{
				return new List<User>();
			}
		}
	}

	public ICollection<Role> Roles
	{
		get
		{
			try
			{
				return dataAccess.GetRoles().Values;
			}
			catch
			{
				return new List<Role>();
			}
		}
	}

	public UserManager(Container container)
	{
		dependencyContainer = container;
		dataAccess = container.Resolve<IAuthenticationDataAccess>();
		eventManager = container.Resolve<IEventManager>();
	}

	public User CreateUser(string userName, string password)
	{
		User result = null;
		try
		{
			User user = new User();
			user.Name = userName;
			user.Password = password;
			User user2 = user;
			dataAccess.CreateUser(user2);
			result = user2;
		}
		catch
		{
		}
		return result;
	}

	public void DeleteUser(User user)
	{
		try
		{
			dataAccess.DeleteUser(user);
		}
		catch
		{
		}
	}

	public User GetUser(Guid userId)
	{
		User result = null;
		try
		{
			result = dataAccess.ReadUser(userId);
		}
		catch
		{
		}
		return result;
	}

	public Role GetRole(Guid roleId)
	{
		Role result = null;
		try
		{
			result = dataAccess.ReadRole(roleId);
		}
		catch
		{
		}
		return result;
	}

	public ICollection<User> GetUsers(Role role)
	{
		ICollection<User> collection = null;
		try
		{
			if (role != null)
			{
				collection = dataAccess.GetUsersByRole(role.Id);
			}
		}
		catch
		{
		}
		if (collection == null)
		{
			collection = new List<User>();
		}
		return collection;
	}

	public Role CreateRole(string roleName)
	{
		return CreateRole(roleName, Guid.Empty);
	}

	public Role CreateRole(string roleName, Guid roleId)
	{
		Role result = null;
		try
		{
			Role role = new Role();
			if (roleId != Guid.Empty)
			{
				role.Id = roleId;
			}
			role.Name = roleName;
			dataAccess.CreateRole(role);
			result = role;
		}
		catch
		{
		}
		return result;
	}

	public void DeleteRole(Role role)
	{
		try
		{
			dataAccess.DeleteRole(role);
		}
		catch
		{
		}
	}

	public void AddUserToRole(User user, Role role)
	{
		try
		{
			dataAccess.AddUserToRole(user, role);
		}
		catch
		{
		}
	}

	public void RemoveUserFromRole(User user, Role role)
	{
		try
		{
			dataAccess.RemoveUserFromRole(user, role);
		}
		catch
		{
		}
	}

	public void SyncRolesAndUsers(IDictionary<Guid, Role> roles, IDictionary<Guid, User> users)
	{
		using DatabaseConnection databaseConnection = dataAccess.OpenDatabaseTransaction();
		try
		{
			IDictionary<Guid, Role> roles2 = dataAccess.GetRoles();
			foreach (Role value in roles2.Values)
			{
				if (!roles.ContainsKey(value.Id))
				{
					Role role = dataAccess.ReadRole(value.Id);
					dataAccess.DeleteRole(role);
					Log.Information(Module.Authentication, "UserManager", $"Role [{role.Name}] with Id {role.Id} was deleted from SHC.");
				}
			}
			foreach (Role value2 in roles.Values)
			{
				if (!roles2.ContainsKey(value2.Id))
				{
					dataAccess.CreateRole(value2);
					Log.Information(Module.Authentication, "UserManager", $"Role [{value2.Name}] with Id {value2.Id} was added to the SHC.");
					continue;
				}
				Role role2 = dataAccess.ReadRole(value2.Id);
				role2.Name = value2.Name;
				dataAccess.UpdateRole(role2);
				Log.Information(Module.Authentication, "UserManager", $"Role [{role2.Name}] with Id {role2.Id} was updated in the SHC.");
			}
			IDictionary<Guid, User> users2 = dataAccess.GetUsers();
			foreach (User value3 in users2.Values)
			{
				if (users.ContainsKey(value3.Id))
				{
					continue;
				}
				User user = dataAccess.ReadUser(value3.Id);
				foreach (Role role3 in user.Roles)
				{
					dataAccess.RemoveUserFromRole(user, role3);
				}
				DeleteUser(user);
				Log.Information(Module.Authentication, "UserManager", $"User [{user.Name}] with Id {user.Id} was deleted from SHC.");
			}
			foreach (User value4 in users.Values)
			{
				if (!users2.ContainsKey(value4.Id))
				{
					value4.LastLoginDate = value4.CreateDate;
					dataAccess.CreateUser(value4);
					foreach (Role role4 in value4.Roles)
					{
						dataAccess.AddUserToRole(value4, role4);
					}
					Log.Information(Module.Authentication, "UserManager", $"User [{value4.Name}] with Id {value4.Id} was added to the SHC.");
					continue;
				}
				User user2 = dataAccess.ReadUser(value4.Id);
				if (value4.Roles.Find((Role role3) => role3.Id == DefaultRoles.Guest.Id || role3.Id == DefaultRoles.ShcUser.Id || role3.Id == DefaultRoles.UserWithoutRights.Id) != null)
				{
					Log.Information(Module.Authentication, $"User {value4.Name} logged out due to updates in roles.");
				}
				foreach (Role role5 in user2.Roles)
				{
					dataAccess.RemoveUserFromRole(user2, role5);
				}
				user2.Name = value4.Name;
				user2.Password = value4.Password;
				user2.CreateDate = value4.CreateDate;
				user2.Roles = value4.Roles;
				dataAccess.UpdateUser(user2);
				foreach (Role role6 in value4.Roles)
				{
					dataAccess.AddUserToRole(value4, role6);
				}
				Log.Information(Module.Authentication, "UserManager", $"User [{value4.Name}] with Id {value4.Id} was updated in the SHC.");
			}
			databaseConnection.CommitTransaction();
		}
		catch (Exception)
		{
			databaseConnection.RollbackTransaction();
			throw;
		}
	}

	public void Initialize()
	{
	}

	public void Uninitialize()
	{
	}
}
