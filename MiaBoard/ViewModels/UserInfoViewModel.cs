using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiaBoard.ViewModels
{
    public class UserInfoViewModel
    {
        public int ID { get; set; }
        [Display(Name = "E-mail address")]
        public string Email { get; set; }
    }
}