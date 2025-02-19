using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Interaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public required String Action { get; set; }
        [Required]
        public required DateTime TimeStamp { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public User? User { get; set; }
        [Required]
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
