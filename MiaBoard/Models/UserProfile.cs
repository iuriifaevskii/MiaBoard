using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MiaBoard.Models
{
    public class UserProfile
    {
        [ForeignKey("AppUser")]
        public int Id { get; set; }

        [Display(Name = "First Name")]
        [StringLength(255)]
        public string FirstName { get; set; }

        [Display(Name = "Midle Name")]
        [StringLength(255)]
        public string MidleName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(255)]
        public string LastName { get; set; }

        public int Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateRegistration { get; set; }

        [Display(Name = "Hired Date")]
        [DataType(DataType.Date)]
        public DateTime DateHired { get; set; }

        [Display(Name = "Contact Number")]
        [StringLength(50)]
        public string ContactNo { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
}