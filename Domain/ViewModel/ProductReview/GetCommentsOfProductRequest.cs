using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.ProductReview
{
    public class GetCommentsOfProductRequest
    {
        public Guid ProductId { get; set; }
        public required string ColorName { get; set; }
    }
}
