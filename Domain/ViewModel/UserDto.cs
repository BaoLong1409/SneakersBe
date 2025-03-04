using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public required string FirstName { get; set; }
        public required string AvatarUrl { get; set; }
        public required string LastName { get; set; }
        public required string LanguageCode { get; set; }
        public required string Theme { get; set; }
    }
}
