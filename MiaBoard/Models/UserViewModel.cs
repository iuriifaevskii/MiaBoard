using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiaBoard.Models
{
    public class UserViewModel
    {
        public int ID { get; set; }
        [Display(Name = "E-mail address")]
        public string Email { get; set; }
        [Display(Name = "Full name")]
        public string FullName { get; set; }
        public IEnumerable<RoleViewModel> Roles { get; set; }
    }
    public class RoleViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Role name")]
        public string Name { get; set; }
    }
    public class AddRoleViewModel
    {
        [Display(Name = "Role name")]
        public string Name { get; set; }
    }
}