using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository User {  get; }
        IProductRepository Product { get; }
        IProductCartRepository ProductCart { get; }
        IProductQuantityRepository ProductQuantity { get; }
        ICartRepository Cart { get; }
        ISizeRepository Size { get; }
        IColorRepository Color { get; }
        IOrderRepository Order { get; }
        IOrderDetailRepository OrderDetail { get; }
        IOrderStatusHistoryRepository OrderStatusHistory { get; }
        int Complete();
    }
}
