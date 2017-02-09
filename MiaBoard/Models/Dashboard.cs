using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiaBoard.Models
{
    public class Dashboard
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual ICollection<AppUser> AppUsers { get; set; }

    }
}