using Domain.Entities;
using Domain.ViewModel.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProductRepository : IGenericRepository<ShowProductsDto>
    {
        public Task<IEnumerable<ImageProductDto>> GetImageProductColors(Guid productId, IEnumerable<Color> colors);
    }
}
