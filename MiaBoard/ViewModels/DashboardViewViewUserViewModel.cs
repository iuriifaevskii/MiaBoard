using MiaBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiaBoard.ViewModels
{
    public class DashboardViewViewUserViewModel
    {
        public Dashboard Dashboard { get; set; }
        public List<Dashboard> DashboardList { get; set; }
        public List<DataSource> DataSources { get; set; }
        public IDictionary<int, string> DashletsSqlResult { get; set; }
        public Dashlet Dashlet { get; set; }
        public List<Dashlet> Dashlets { get; set; }
        public List<Dashlet> DashletsFirstCol { get; set; }
        public List<Dashlet> DashletsSecondCol { get; set; }
        public List<Dashlet> DashletsThirdCol { get; set; }
    }
}