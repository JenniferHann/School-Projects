using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoNameBikes.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime TimeStamp { get; set; }

        [Required(ErrorMessage = "This is required.")]
        [Display(Name = "First Name")]
        [StringLength(50, MinimumLength = 2)]
        [RegularExpression(@"^[a-zA-Z$]{2,50}$", ErrorMessage = "The field First Name must be a string with a minimum length of 2 and a maximum length of 50.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "This is required.")]
        [Display(Name = "Last Name")]
        [StringLength(50, MinimumLength = 2)]
        [RegularExpression(@"^[a-zA-Z]{2,50}$", ErrorMessage = "The field Last Name must be a string with a minimum length of 2 and a maximum length of 50.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "This is required.")]
        [StringLength(30, MinimumLength = 2)]
        [RegularExpression("[ABCDEFGHIJKLMNOPQRSTUVWXYZ][0-9][ABCDEFGHIJKLMNOPQRSTUVWXYZ] ?[0-9][ABCDEFGHIJKLMNOPQRSTUVWXYZ][0-9]", ErrorMessage = "Error! Must be a valid postal code (X1X 1X1)")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "This is required.")]
        [RegularExpression("[A-Za-z0-9.]+(?:.[]+)*@([A-Za-z]+)([.])[a-z](?:[a-z-]*[a-z])?", ErrorMessage = "The Email field is not a valid e-mail address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This is required.")]
        public string Topic { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "This is required.")]
        public string Comment { get; set; }
    }
}
