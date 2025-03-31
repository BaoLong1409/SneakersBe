using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.Category
{
    public class CategoryDto
    {
        public Guid CategoryId { get; set; }
        public required string CategoryName { get; set; }
        public required string Brand { get; set; }

    }
}
