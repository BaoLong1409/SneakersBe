using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OrderStatusHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public required String Status { get; set; }
        public String? Note { get; set; }
        [Required]
        public required DateTime UpdatedAt { get; set; }
        [Required]
        public Guid OrderId { get; set; }
        public Order? Order { get; set; }
    }
}
