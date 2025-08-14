using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.Core.Authentication.Entities;
using RWE.SmartHome.SHC.Core.Database;

namespace RWE.SmartHome.SHC.Core.Authentication;

public interface IAuthenticationDataAccess : IService
{
	Guid InternalUserId { get; }

	DatabaseConnection OpenDatabaseTransaction();

	void AddUserToRole(User user, Role role);

	void CreateRole(Role role);

	void CreateUser(User user);

	void DeleteRole(Role role);

	void DeleteUser(User user);

	IDictionary<Guid, Role> GetRoles();

	IList<Role> GetRolesByUser(Guid userId);

	IList<User> GetUsersByRole(Guid roleId);

	IDictionary<Guid, User> GetUsers();

	Role ReadRole(Guid roleId);

	User ReadUser(Guid userId);

	User ReadUser(string userName);

	void RemoveUserFromRole(User user, Role role);

	void UpdateRole(Role role);

	void UpdateUser(User user);
}
