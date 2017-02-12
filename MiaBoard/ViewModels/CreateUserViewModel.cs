using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MiaBoard.ViewModels
{
    public class CreateUserViewModel
    {
        [Required, Display(Name = "E-mail address"), EmailAddress]
        public string Email { get; set; }
        
        [Display(Name = "First Name")]
        [Required, StringLength(maximumLength: 100)]
        public string FirstName { get; set; }

        [Display(Name = "Midle Name")]
        [Required, StringLength(maximumLength: 100)]
        public string MidleName { get; set; }

        [Display(Name = "Last Name")]
        [Required, StringLength(maximumLength: 100)]
        public string LastName { get; set; }

        public int Gender { get; set; }

        [Display(Name = "Registration Date")]
        [Column(TypeName = "DateTime2")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ? DateRegistration { get; set; }

        [Display(Name = "Hired Date")]
        [Column(TypeName = "DateTime2")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ? DateHired { get; set; }

        [Display(Name = "Contact Number")]
        [StringLength(50)]
        public string ContactNo { get; set; }

        [Required, Display(Name = "Password"), StringLength(maximumLength: 20, ErrorMessage = "{0} maximum length, {2} minimum lendth", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password), Display(Name = "Confirm password"), Compare("Password", ErrorMessage = "Пароль має співпадати")]
        public string ConfirmPassword { get; set; }

        [Required, Display(Name = "Select role")]
        public int RoleId { get; set; }

    }
}