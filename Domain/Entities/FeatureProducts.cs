using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class FeatureProducts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public required string ImageUrl { get; set; }
        [Required]
        public required string LeftColor { get; set; }
        [Required]
        public required string MiddleColor { get; set; }
        [Required]
        public required string RightColor { get; set; }
        [Required]
        public required Guid ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
