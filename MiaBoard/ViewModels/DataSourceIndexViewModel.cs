using MiaBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiaBoard.ViewModels
{
    public class DataSourceIndexViewModel
    {
        public DataSource NewDataSource { get; set; }
        public IEnumerable<DataSource> DataSources { get; set; }
    }
}