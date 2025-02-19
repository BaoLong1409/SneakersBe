using DataAccess.DbContext;
using Domain.Interfaces;
using Domain.ViewModel;
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
    }
}
