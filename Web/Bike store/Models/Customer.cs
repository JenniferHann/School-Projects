using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoNameBikes.Models
{
    public partial class Customer
    {
        public Customer()
        {
            CustomerAddress = new HashSet<CustomerAddress>();
            SalesOrderHeader = new HashSet<SalesOrderHeader>();
        }

        [Key]
        public int CustomerId { get; set; }
        public bool NameStyle { get; set; }
        public string Title { get; set; }

        [Required]
        [Display (Name = "First Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "The field First Name must be a string with a minimum length of 2 and a maximum length of 50.")]
        public string FirstName { get; set; }

        [Display (Name = "Middle Name")]
        [StringLength(50, ErrorMessage = "The field Middle Name cannot exceed 50 characters")]
        public string MiddleName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "The field Last Name must be a string with a minimum length of 2 and a maximum length of 50.")]
        public string LastName { get; set; }
        public string Suffix { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        public string SalesPerson { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Error! Must be a valid e-mail adress.")]
        public string EmailAddress { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "This is required")]
        [DataType(DataType.Password)]
        [NotMapped]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "Must be between 5 and 255 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "This is required")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The fields Password and Confirm Password should be equals")]
        [NotMapped]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "Must be between 5 and 255 characters")]
        public string ConfirmPassword { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public Guid Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<CustomerAddress> CustomerAddress { get; set; }
        public virtual ICollection<SalesOrderHeader> SalesOrderHeader { get; set; }
    }
}
