using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.User
{
    public class GoogleLoginRequest
    {
        public required string GoogleToken { get; set; }
    }
}
