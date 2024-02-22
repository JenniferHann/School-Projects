using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NoNameBikes.Models
{
    public partial class LogIn
    {
        [Key]
        public int LoginId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime TimeStamp { get; set; }

    }
}
