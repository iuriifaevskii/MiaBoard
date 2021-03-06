﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace MiaBoard.ViewModels
{
    class UserEditYaroslavViewModel
    {
        public int ID { get; set; }
        [Required, Display(Name = "E-mail address"), EmailAddress]
        public string Email { get; set; }

        [Required, Display(Name = "First Name")]
        public string Name { get; set; }

        [Required, Display(Name = "Midle Name")]
        public string MidleName { get; set; }

        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }

        public int Gender { get; set; }

        [Display(Name = "Hired Date")]
        [Column(TypeName = "DateTime2")]
        public DateTime? DateHired { get; set; }

        [Required, Display(Name = "Contact Number")]
        public string ContactNo { get; set; }

        [Required, Display(Name = "Select role")]
        public int RoleId { get; set; }

        [Required, Display(Name = "Odl password"), StringLength(maximumLength: 20, ErrorMessage = "{0} maximum length, {2} minimum lendth", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required, Display(Name = "Password"), StringLength(maximumLength: 20, ErrorMessage = "{0} maximum length, {2} minimum lendth", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password), Display(Name = "Confirm password"), Compare("Password", ErrorMessage = "Пароль має співпадати")]
        public string PasswordSalt { get; set; }
    }
}
