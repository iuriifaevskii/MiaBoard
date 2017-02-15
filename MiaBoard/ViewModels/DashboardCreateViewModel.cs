using MiaBoard.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiaBoard.ViewModels
{
    public class DashboardCreateViewModel
    {

        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Title { get; set; }

        [Display(Name = "Company Admin")]
        public int IdOwner { get; set; }

        [Display(Name = "Dashboard Admin")]
        public int IdDashboardAdmin { get; set; }

        public List<AppUser> CandidatsAdmin { get; set; }
        public List<AppUser> CandidatsOwner { get; set; }

    }
}