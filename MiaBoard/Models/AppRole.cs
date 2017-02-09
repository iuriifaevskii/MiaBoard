using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiaBoard.Models
{
    public class AppRole
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(maximumLength: 255)]
        public string Name { get; set; }
        public virtual ICollection<AppUser> AppUsers { get; set; }
    }
}