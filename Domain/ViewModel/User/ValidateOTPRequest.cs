using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.User
{
    public class ValidateOTPRequest
    {
        public required string Email { get; set; }
        public required string OTP { get; set; }
    }
}
