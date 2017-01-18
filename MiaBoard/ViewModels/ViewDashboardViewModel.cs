﻿using MiaBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiaBoard.ViewModels
{
    public class ViewDashboardViewModel
    {
        public Dashboard Dashboard { get; set; }
        public List<Dashlet> Dashlets { get; set; }
    }
}