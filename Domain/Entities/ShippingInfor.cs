using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ShippingInfor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public required string FullName { get; set; }
        [EmailAddress]
        public string? Email {  get; set; }
        [Required]
        [RegularExpression(@"^(03|05|07|08|09)\d{8}$")]
        public required string PhoneNumber { get; set; }
        [Required]
        public required string Address { get; set; }
        public string? Note { get; set; }
        [Required]
        public int IsMainAddress { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}
