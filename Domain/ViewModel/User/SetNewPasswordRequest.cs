using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.User
{
    public class SetNewPasswordRequest
    {
        public required string NewPassword { get; set; }
    }
}
