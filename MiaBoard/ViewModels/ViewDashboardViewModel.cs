using MiaBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiaBoard.ViewModels
{
    public class ViewDashboardViewModel
    {
        public Dashboard Dashboard { get; set; }
        public List<Dashboard> DashboardList { get; set; }
 
        public IDictionary<int, string> DashletsSqlResult { get; set; }
        //yura comment
        public Dashlet Dashlet { get; set; }

        //yura comment
        public List<Dashlet> Dashlets { get; set; }

        public List<Dashlet> DashletsFirstCol { get; set; }
        public List<Dashlet> DashletsSecondCol { get; set; }
        public List<Dashlet> DashletsThirdCol { get; set; }
    }
}