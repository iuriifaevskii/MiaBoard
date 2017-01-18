using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiaBoard.Models
{
    public class DataSource
    {
        public string ConnectionString { get; set; }

        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter customer's name.")]
        [StringLength(255)]
        [Display(Name = "Type")]
        public string Type { get; set; }

        [StringLength(255)]
        [Display(Name = "Server Name")]
        public string ServerName { get; set; }

        [StringLength(255)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [StringLength(255)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [StringLength(255)]
        [Display(Name = "Database Name")]
        public string DatabaseName { get; set; }

    }
}