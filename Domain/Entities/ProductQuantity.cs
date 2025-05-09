﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProductQuantity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public Guid SizeId { get; set; }
        public Size? Size { get; set; }
        [Required]
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }
        [Required]  
        public Guid ColorId { get; set; }
        public Color? Color { get; set; }
        [Required]
        public int StockQuantity {  get; set; }
    }
}
