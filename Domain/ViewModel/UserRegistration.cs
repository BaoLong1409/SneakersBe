using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class UserRegistration
    {
        [Required]
        public required String FirstName { get; set; }
        [Required]
        public required String LastName { get; set; }
        [Required, EmailAddress]
        public required string Email { get; set; }
        [Required]
        public required string PhoneNumber { get; set; }
        [DataType(DataType.Password)]
        public required string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password")]
        public required string ConfirmPassword { get; set; }
    }
}
