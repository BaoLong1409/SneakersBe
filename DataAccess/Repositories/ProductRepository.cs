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
    public class ProductRepository : GenericRepository<ShowProductsDto>, IProductRepository
    {
        public ProductRepository(SneakersDbContext context) : base(context)
        {
            
        }

        public async Task<IEnumerable<ImageProductDto>> GetImageProductColors(Guid productId, IEnumerable<Color> colors)
        {
            var colorIds = colors.Select(c => c.Id).ToList();

            var images = await _context.ProductImage
                .Where(pi => pi.ProductId == productId && colorIds.Contains(pi.ColorId) && pi.IsThumbnail == 1)
                .Select(pi => new ImageProductDto
                {
                    ImageId = pi.Id,
                    ImageUrl = pi.ImageUrl,
                    ProductId = pi.ProductId,
                    IsThumbnail = pi.IsThumbnail,
                    ColorId = pi.ColorId
                })
                .ToListAsync();
            foreach (var image in images) {
                image.ColorName = colors.FirstOrDefault(c => c.Id == image.ColorId).Name;
            }

            return images;
        }
    }
}
