using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.User
{
    public class ShippingInfoDto
    {
        public Guid? Id { get; set; }
        public required String FullName { get; set; }
        public String? Email { get; set; }
        [RegularExpression(@"^(03|05|07|08|09)\d{8}$")]
        public required String PhoneNumber { get; set; }
        public required String Address { get; set; }
        public String? Note { get; set; }
        public int IsMainAddress { get; set; }

        public Guid UserId { get; set; }
    }
}
