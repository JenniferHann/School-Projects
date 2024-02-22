using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NoNameBikes.Models
{
    public partial class PasswordRecoveryToken
    {
        [Key]
        public int TokenId { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreationDate { get; set; }

        public bool Expired { get; set; }
    }
}
