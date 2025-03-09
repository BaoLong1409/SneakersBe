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
    public class OrderStatusHistoryRepository : GenericRepository<OrderStatusHistory>, IOrderStatusHistoryRepository
    {
        public OrderStatusHistoryRepository(SneakersDbContext context) : base(context)
        {
        }
    }
}
