using System;
using System.Collections.Generic;
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
        public required string PhoneNumber { get; set; }
    }
}
