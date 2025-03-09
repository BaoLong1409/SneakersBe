using DataAccess.DbContext;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ViewModel.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ProductQuantityRepository : GenericRepository<ProductQuantity>, IProductQuantityRepository
    {
        public ProductQuantityRepository(SneakersDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<AvailableProductsDto>> GetAvailableProducts(Guid productId, Guid colorId)
        {
            return await _context.ProductQuantity
                .Where(pq => pq.ProductId == productId && pq.ColorId == colorId)
                .Select(pq => new AvailableProductsDto
                {
                    SizeId = pq.SizeId,
                    SizeNumber = pq.Size.SizeNumber,
                    Quantity = pq.StockQuantity
                }).ToListAsync();
        }
    }
}
