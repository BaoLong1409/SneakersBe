using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OrderDetail
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public required int Quantity { get; set; }
        [Required]
        public Decimal PriceAtOrder { get; set; }
        [Required]
        public required Guid OrderId { get; set; }
        public Order? Order { get; set; }
        [Required]
        public required Guid ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
