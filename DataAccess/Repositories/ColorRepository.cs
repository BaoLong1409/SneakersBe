using DataAccess.DbContext;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ColorRepository : GenericRepository<Color>, IColorRepository
    {
        public ColorRepository(SneakersDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Color?>> GetProductColors(Guid productId)
        {
            var colors = await _context.ProductQuantity
                .Where(pq => pq.ProductId == productId)
                .Include(pq => pq.Color)
                .Select(pq => pq.Color)
                .Distinct()
                .ToListAsync();

            return colors.AsEnumerable();
        }
    }
}
