using AutoMapper;
using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces;
using Domain.ViewModel.Order;
using System.Transactions;

namespace Sneakers.Services.OrderService
{
    public class OrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public (EnumOrder status, Guid? orderId) AddOrder(CreateOrderRequest orderRequest)
        {
            if (orderRequest.Order == null)
            {
                return (EnumOrder.CreateOrderFail, null);
            }

            if (orderRequest.Order.ShippingAddress == null && orderRequest.Order.ShippingInforId == null)
            {
                return (EnumOrder.NeedAddress, null);
            }

            if (orderRequest.OrderDetails == null) {
                return (EnumOrder.CreateOrderFail, null);
            }

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var order = _mapper.Map<Order>(orderRequest.Order);
                List<OrderDetail> orderDetails = _mapper.Map<List<OrderDetail>>(orderRequest.OrderDetails);

                var orderHistory = new OrderStatusHistoryDto
                {
                    Status = "Pending",
                    Note = "Waitting Confirm From Store",
                    UpdatedAt = DateTime.Now,
                };
                var orderStatusHistory = _mapper.Map<OrderStatusHistory>(orderHistory);

                try
                {
                    _unitOfWork.Order.Add(order);
                    _unitOfWork.Complete();

                    orderDetails.ForEach(x => x.OrderId = order.Id);
                    _unitOfWork.OrderDetail.AddRange(orderDetails);
                    _unitOfWork.Complete();

                    orderStatusHistory.OrderId = order.Id;
                    _unitOfWork.OrderStatusHistory.Add(orderStatusHistory);
                    _unitOfWork.Complete();
                    transaction.Complete();
                    return (EnumOrder.CreateOrderSuccess, order.Id);
                }
                catch (Exception ex)
                {
                    return (EnumOrder.CreateOrderFail, null);
                }
            }

            

        }
    }
}
