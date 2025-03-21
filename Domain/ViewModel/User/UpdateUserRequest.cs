using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.User
{
    public class UpdateUserRequest
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public string? AvatarFile { get; set; }
        public string? AvatarName { get; set; }
        public required string LastName { get; set; }
        [RegularExpression(@"^(03|05|07|08|09)\d{8}$", ErrorMessage = "Your phone number is invalid.")]
        public required string PhoneNumber { get; set; }
    }
}
