using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        [Required]
        public required String FirstName { get; set; }
        [Required]
        public required String LastName { get; set; }
        [Required]
        public String? Theme { get; set; } = "light";
        [Required]
        public String? Language { get; set; } = "en";
        
        public String? AvatarUrl { get; set; }

        public List<ShippingInfor>? ShippingInfor { get; set; }
    }
}
