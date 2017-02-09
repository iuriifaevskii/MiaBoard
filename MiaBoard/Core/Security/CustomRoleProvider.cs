using MiaBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MiaBoard.Core.Security
{
    public class CustomRoleProvider : RoleProvider
    {
        private ApplicationDbContext _context;
        public CustomRoleProvider()
        {
            _context = new ApplicationDbContext();
        }
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

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
            return GetRolesForUserForRoles(username);
        }
        public string[] GetRolesForUserForRoles(string email)
        {
            var user = this.FindUserByEmail(email);
            if (user != null)
            {
                return user.Roles.Select(r => r.Name).ToArray();
            }
            return null;
        }
        public AppUser FindUserByEmail(string email)
        {
            AppUser user = null;
            user = _context.AppUsers.AsQueryable().SingleOrDefault(u => u.Email == email);
            return user;
        }
        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
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