using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiaBoard.Dtos
{
    public class DataSourceDto
    {
        public string ConnectionString { get; set; }

        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Type { get; set; }

        [StringLength(255)]
        public string ServerName { get; set; }

        [StringLength(255)]
        public string UserName { get; set; }

        [StringLength(255)]
        public string Password { get; set; }

        [StringLength(255)]
        public string DatabaseName { get; set; }


    }
}