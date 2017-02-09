using MiaBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiaBoard.ViewModels
{
    public class AddUserToDashboardViewModel
    {
        public IEnumerable<ListBoxItems> ListUsers { get; set; }
        public IEnumerable<ListBoxItems> ListDashboards { get; set; }

        public int UserIdSected { get; set; }
        public int DashboardIdSected { get; set; }
    }
}