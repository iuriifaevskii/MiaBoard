﻿using MiaBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiaBoard.ViewModels
{
    public class DataSourceIndexViewModel
    {
        public AppUser CurrentUser { get; set; }
        public _DataSourcesCreateViewModel NewDataSource { get; set; }
        public DataSource UpdateDataSource { get; set; }
        public IEnumerable<DataSource> DataSources { get; set; }

        public bool IsCompanyAdmin { get; set; }

        public bool IsSuperAdmin { get; set; }
    }
}