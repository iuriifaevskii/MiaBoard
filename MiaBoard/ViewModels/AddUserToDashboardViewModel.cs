using MiaBoard.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiaBoard.ViewModels
{
    public class AddUserToDashboardViewModel
    {
        public IEnumerable<ListBoxItems> ListUsers { get; set; }
        public IEnumerable<ListBoxItems> ListDashboards { get; set; }

        [Display(Name = "Select User")]
        public int UserIdSected { get; set; }
        [Display(Name = "Attach Dashboard")]
        public int DashboardIdSected { get; set; }

    }
}