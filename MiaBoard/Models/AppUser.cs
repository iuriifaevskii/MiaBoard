using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiaBoard.Models
{
    public class AppUser
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(maximumLength: 255)]
        [EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(maximumLength: 200)]
        public string Password { get; set; }

        [Required, StringLength(maximumLength: 150)]
        public string PasswordSalt { get; set; }

        public virtual UserProfile UserProfile { get; set; }
        public virtual ICollection<AppRole> Roles { get; set; }
        public virtual ICollection<Dashboard> Dashboards { get; set; }


    }
}