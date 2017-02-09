using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MiaBoard.Models
{
    public class LoginViewModel
    {
        [Required, Display(Name = "Login")]
        public string Login { get; set; }
        [Required, DataType(DataType.Password), Display(Name = "Password")]
        public string Password { get; set; }
        [Display(Name = "Remember me?")]
        public bool IsRememberMe { get; set; }
    }

}
