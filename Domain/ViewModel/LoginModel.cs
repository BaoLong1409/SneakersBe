using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class LoginModel
    {
        [Required]
        public required string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
