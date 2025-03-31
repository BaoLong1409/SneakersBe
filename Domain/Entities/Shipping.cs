using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Shipping
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public required string ShippingName { get; set; }
        [Required]
        public Decimal Price { get; set; }
        [Required]
        public required int MinimumDeliveredTime { get; set; }
        [Required]
        public required int MaximumDeliveredTime { get; set; }
    }
}
