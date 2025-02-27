using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IColorRepository : IGenericRepository<Color>
    {
        public Task<IEnumerable<Color?>> GetProductColors(Guid productId);
    }
}
