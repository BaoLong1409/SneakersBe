using DataAccess.DbContext;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ViewModel.Cart;
using Domain.ViewModel.Order;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        private readonly SneakersDbContext _context;
        public OrderDetailRepository(SneakersDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
