using MiaBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiaBoard.ViewModels
{
    public class UserIndexViewModel
    {
        public List<AppUser> UsersList { get; set; }   
        public AppUser CurrentUser { get; set; }

        public bool IsCompanyAdmin { get; set; }

        public bool IsSuperAdmin { get; set; }

        public bool IsUser { get; set; }
    }
}