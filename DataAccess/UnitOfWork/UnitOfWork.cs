using DataAccess.DbContext;
using DataAccess.Repositories;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SneakersDbContext _context;
        public IUserRepository User{  get; private set; }
        public IProductRepository Product { get; private set; }
        public UnitOfWork(SneakersDbContext context)
        {
            _context = context;
            User = new UserRepository(_context);
            Product = new ProductRepository(_context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
