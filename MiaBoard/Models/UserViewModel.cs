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
        public int? Gender { get; set; }
        public DateTime? DateRegistration { get; set; }
        public DateTime? DateHired { get; set; }
        public string ContactNo { get; set; }
        public IEnumerable<RoleViewModel> Roles { get; set; }
        //public IEnumerable<Dashboard> Dashboards { get; set; }
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