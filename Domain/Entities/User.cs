using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        [Required]
        public required string FirstName { get; set; }
        [Required]
        public required string LastName { get; set; }
        [Required]
        public string? Theme { get; set; } = "light";
        [Required]
        public string? Language { get; set; } = "en";

        public List<ShippingInfor>? ShippingInfor { get; set; }
    }
}
