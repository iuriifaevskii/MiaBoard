using MiaBoard.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiaBoard.ViewModels
{
    public class AddUserToRoleViewModel
    {
        public IEnumerable<ListBoxItems> ListUsers { get; set; }
        public IEnumerable<ListBoxItems> ListRoles { get; set; }

        [Display(Name = "Select User ")]
        public int UserIdSected { get; set; }
        [Display(Name = "Attach Role")]
        public int RoleIdSected { get; set; }
    }
}