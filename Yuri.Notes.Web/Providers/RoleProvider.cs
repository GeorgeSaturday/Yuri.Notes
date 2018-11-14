using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Yuri.Notes.DB;

namespace Yuri.Notes.Web.Providers
{
    public class RoleProvider : System.Web.Security.RoleProvider
    {
        public RoleProvider()
        {
            UserRepository = new NHUserRepository();
        }

        IUserRepository UserRepository { get; set; }

        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            var user = UserRepository.LoadByName(username);

            if (user != null && user.Role != null)
            {
                return new[] { user.Role.Name };
            }
            return new string[0];
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            var user = UserRepository.LoadByName(username);
          

            return user != null && user.Role != null && user.Role.Name == roleName;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}