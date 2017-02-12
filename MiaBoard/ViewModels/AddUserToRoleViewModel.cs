using MiaBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiaBoard.ViewModels
{
    public class AddUserToRoleViewModel
    {
        public IEnumerable<ListBoxItems> ListUsers { get; set; }
        public IEnumerable<ListBoxItems> ListRoles { get; set; }

        public int UserIdSected { get; set; }
        public int RoleIdSected { get; set; }
    }
}