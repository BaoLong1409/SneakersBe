using DataAccess.DbContext;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ProductReviewImageRepository : GenericRepository<ProductReviewImage>, IProductReviewImageRepository
    {
        public ProductReviewImageRepository(SneakersDbContext context) : base(context)
        {
        }
    }
}
