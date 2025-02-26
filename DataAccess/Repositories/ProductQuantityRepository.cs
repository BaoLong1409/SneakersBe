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
    public class ProductQuantityRepository : GenericRepository<ProductQuantity>, IProductQuantityRepository
    {
        public ProductQuantityRepository(SneakersDbContext context) : base(context)
        {
        }
    }
}
