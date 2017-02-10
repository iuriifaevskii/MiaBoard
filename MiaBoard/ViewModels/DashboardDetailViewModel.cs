using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiaBoard.ViewModels
{
    public class DashboardDetailViewModel
    {
        public int ID { get; set; }
        [Display(Name = "E-mail address")]
        public string Email { get; set; }

        public IEnumerable<DashboardItemViewModel> Dashboards { get; set; }
    }
}