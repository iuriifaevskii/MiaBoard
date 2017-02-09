using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiaBoard.ViewModels
{
    public class UserEditViewModel
    {
        public int ID { get; set; }
        [Required, Display(Name = "E-mail address"), EmailAddress]
        public string Email { get; set; }

        [Required, Display(Name = "Odl password"), StringLength(maximumLength: 20, ErrorMessage = "{0} maximum length, {2} minimum lendth", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required, Display(Name = "Password"), StringLength(maximumLength: 20, ErrorMessage = "{0} maximum length, {2} minimum lendth", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password), Display(Name = "Confirm password"), Compare("Password", ErrorMessage = "Пароль має співпадати")]
        public string ConfirmPassword { get; set; }
    }
}