using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public required int Reviewed {  get; set; }
        [Required]
        public required Guid OrderId { get; set; }
        public Order? Order { get; set; }
        [Required]
        public required Guid ColorId { get; set; }
        public Color? Color { get; set; }
        [Required]
        public required Guid SizeId { get; set; }
        public Size? Size { get; set; }
        [Required]
        public required Guid ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
