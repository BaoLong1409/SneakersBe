using DataAccess.DbContext;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ProductCartRepository : GenericRepository<ProductCart>, IProductCartRepository
    {
        private readonly SneakersDbContext _context;
        public ProductCartRepository(SneakersDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductInCartDto>> GetProductInCartsAsync(Guid cartId)
        {
            return await _context.ProductCart
                .Where(pc => pc.CartId == cartId)
                .Select(pc => new ProductInCartDto
                {
                    ProductId = pc.ProductId,
                    ColorId = pc.ColorId,
                    SizeId = pc.SizeId,
                    Name = pc.Product.Name,
                    Price = pc.Product.Price,
                    ImageUrl = pc.Product.ProductImages
                        .Where(pi => pi.IsThumbnail == 1 && pi.ColorId == pc.ColorId)
                        .Select(pi => pi.ImageUrl)
                        .FirstOrDefault(),
                    ColorName = pc.Color.Name,
                    SizeNumber = pc.Size.SizeNumber,
                    Quantity = pc.Quantity
                }).ToListAsync();
        }
    }
}
