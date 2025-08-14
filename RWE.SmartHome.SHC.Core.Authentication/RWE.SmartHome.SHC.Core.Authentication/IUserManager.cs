using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.Core.Authentication.Entities;

namespace RWE.SmartHome.SHC.Core.Authentication;

public interface IUserManager : IService
{
	ICollection<User> Users { get; }

	ICollection<Role> Roles { get; }

	User CreateUser(string userName, string password);

	void DeleteUser(User user);

	ICollection<User> GetUsers(Role role);

	User GetUser(Guid userId);

	Role GetRole(Guid roleId);

	Role CreateRole(string roleName);

	Role CreateRole(string roleName, Guid roleId);

	void DeleteRole(Role role);

	void AddUserToRole(User user, Role role);

	void RemoveUserFromRole(User user, Role role);

	void SyncRolesAndUsers(IDictionary<Guid, Role> roles, IDictionary<Guid, User> users);
}
