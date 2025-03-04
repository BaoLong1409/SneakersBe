using Domain.Entities;
using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProductQuantityRepository : IGenericRepository<ProductQuantity>
    {
        public Task<IEnumerable<AvailableProductsDto>> GetAvailableProducts(Guid productId, Guid colorId);
    }
}
