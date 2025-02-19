using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Currency
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required Guid Id { get; set; }
        [Required]
        public required String Code { get; set; }
        [Required]
        public required String Symbol { get; set; }
        [Required]
        public required float ExchangeRate { get; set; }
    }
}
