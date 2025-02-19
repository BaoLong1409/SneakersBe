using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class FeatureProductModel
    {
        public required string ImageUrl { get; set; }
        public required string Name { get; set; }
        public required Decimal Price { get; set; }
        public int? Rating { get; set; }
        public required string LeftColor { get; set; }
        public required string MiddleColor { get; set; }
        public required string RightColor { get; set; }
    }
}
