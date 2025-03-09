using Domain.Entities;
using Domain.ViewModel.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProductCartRepository : IGenericRepository<ProductCart>
    {
        Task<IEnumerable<ProductInCartDto>> GetProductInCartsAsync(Guid cartId);
    }
}
