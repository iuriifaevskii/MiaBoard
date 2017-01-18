using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiaBoard.Models
{
    public class Dashlet
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public Dashboard Dashboard { get; set; }

        public int DashboardId { get; set; }

        public DataSource DataSource { get; set; }

        public int DataSourceId { get; set; }

        public string Sql { get; set; }

        public string Styles { get; set; }
    }
}