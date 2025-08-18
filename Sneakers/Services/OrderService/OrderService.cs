using AutoMapper;
using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces;
using Domain.ViewModel.Order;
using MediatR;
using Sneakers.Features.Queries.Order;
using Sneakers.Services.VnpayService;
using System.Transactions;

namespace Sneakers.Services.OrderService
{
    public class OrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<(EnumOrder status, Guid? orderId)> AddOrder(CreateOrderRequest orderRequest)
        {
            if (orderRequest.Order == null)
            {
                return (EnumOrder.CreateOrderFail, null);
            }

            if (orderRequest.OrderDetails == null)
            {
                return (EnumOrder.CreateOrderFail, null);
            }

            Decimal totalMoney = 0;
            foreach(var orderDetail in orderRequest.OrderDetails)
            {
                totalMoney += (decimal) orderDetail.PriceAtOrder * orderDetail.Quantity;
            }

            orderRequest.Order.TotalMoney = totalMoney;

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string paymentName = null;
                    if (orderRequest.Order.PaymentId.HasValue)
                    {
                        var payment = await _unitOfWork.Payment.GetByIdAsync(orderRequest.Order.PaymentId.Value);
                        paymentName = payment?.PaymentName;
                    }

                    var order = _mapper.Map<Order>(orderRequest.Order);
                    List<OrderDetail> orderDetails = _mapper.Map<List<OrderDetail>>(orderRequest.OrderDetails);

                    var orderHistory = new OrderStatusHistoryDto
                    {
                        Status = paymentName switch
                        {
                            null => "Pending",
                            "COD" => "Success",
                            "VnPay" => "Waiting for Payment",
                            _ => "Unknown Payment Status"
                        },
                        StatusNote = paymentName switch
                        {
                            null => "Waiting for user to select payment method",
                            "COD" => "Order Successfully",
                            "VnPay" => "Payment is being processed",
                            _ => "Unknown payment method"
                        },
                        UpdatedAt = DateTime.Now,
                    };

                    var orderStatusHistory = _mapper.Map<OrderStatusHistory>(orderHistory);

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

        public async Task<IEnumerable<OrdersDto>> GetAllOrdersSv(Guid userId)
        {
            return await _mediator.Send(new GetOrders(userId, 0, 0, null));
        }

        public async Task<IEnumerable<OrdersDto>> GetOrdersByLimit(int offset, int limit, string status)
        {
            return await _mediator.Send(new GetOrders(null, offset, limit, status));
        }

        public async Task<Order?> GetOrderById(Guid orderId)
        {
            return await _unitOfWork.Order.GetFirstOrDefaultAsync(x => x.Id == orderId);
        }

        public async Task<OrderInfoDto> GetOrderInfo(Guid orderId)
        {
            return await _mediator.Send(new GetOrderInfo(orderId));
        }

        public async Task <EnumOrder> UpdateOrder(OrderUpdateRequest orderUpdateReq)
        {
            var existingOrder = await _unitOfWork.Order.GetByIdAsync(orderUpdateReq.OrderId);
            if (existingOrder == null) return EnumOrder.OrderNotFound;
            UpdateOrderStatus(orderUpdateReq.OrderId, "Success", "Order Successfully");
            var payMethod = await _unitOfWork.Payment.GetFirstOrDefaultAsync(x => x.Id == orderUpdateReq.PaymentId);

            if (payMethod.PaymentName == "COD" && payMethod != null)
            {
                var updateQuantity = await UpdateProductQuantity(orderUpdateReq.OrderId);
                if (!updateQuantity)
                {
                    return EnumOrder.UpdateQuantityFail;
                }
            }
            _mapper.Map(orderUpdateReq, existingOrder);
            _unitOfWork.Complete();
            return EnumOrder.UpdateOrderSuccess;
        }

        public async Task<EnumOrder> UpdateOrderStatus(UpdateOrderStatusRequest request)
        {
            var existingOrder = await _unitOfWork.Order.GetByIdAsync(request.OrderId);
            if (existingOrder == null) return EnumOrder.OrderNotFound;
            string statusNote = request.OrderStatus switch
            {
                "Pending" => "Waiting for user to select payment method",
                "Success" => "Order Successfully",
                "Delivering" => "Order has been delivering",
                "Delivered" => "Order has been delivered successfully",
                _ => "Unknown Order Status"
            };

            var updateStatus = UpdateOrderStatus(request.OrderId, request.OrderStatus, statusNote);
            if (updateStatus == EnumOrder.OrderNotFound) return EnumOrder.OrderNotFound;
            return EnumOrder.UpdateOrderSuccess;
        }

        public async Task<EnumOrder> UpdateOrderSuccessPayment(Guid orderId)
        {
            var orderStatusDto = new OrderStatusHistoryDto
            {
                Status = "Success",
                StatusNote = "Pay Successfully",
                UpdatedAt = DateTime.Now,
                OrderId = orderId
            };

            if (orderStatusDto == null)
            {
                return EnumOrder.OrderNotFound;
            }

            var orderStatus = _mapper.Map<OrderStatusHistory>(orderStatusDto);

            _unitOfWork.OrderStatusHistory.Add(orderStatus);

            var updateQuantity = await UpdateProductQuantity(orderId);
            if (!updateQuantity)
            {
                return EnumOrder.UpdateQuantityFail;
            }

            _unitOfWork.Complete();
            return EnumOrder.AddOrderStatusSuccess;
        }

        private EnumOrder UpdateOrderStatus(Guid orderId, string status, string note)
        {
            var orderStatusDto = new OrderStatusHistoryDto
            {
                Status = status,
                StatusNote = note,
                UpdatedAt = DateTime.Now,
                OrderId = orderId
            };

            if (orderStatusDto == null)
            {
                return EnumOrder.OrderNotFound;
            }

            var orderStatus = _mapper.Map<OrderStatusHistory>(orderStatusDto);

            _unitOfWork.OrderStatusHistory.Add(orderStatus);
            _unitOfWork.Complete();
            return EnumOrder.AddOrderStatusSuccess;
        }

        private async Task<bool> UpdateProductQuantity(Guid orderId)
        {
            var orderDetail = await _unitOfWork.OrderDetail.FindAsync(x => x.OrderId == orderId);
            if (orderDetail == null)
            {
                return false;
            }
            foreach (var item in orderDetail)
            {
                var productQuantity = await _unitOfWork.ProductQuantity.GetFirstOrDefaultAsync(x => x.ProductId == item.ProductId && x.ColorId == item.ColorId && x.SizeId == item.SizeId);
                if (productQuantity != null)
                {
                    if (productQuantity.StockQuantity - item.Quantity >=0)
                    {
                        productQuantity.StockQuantity -= item.Quantity;
                    } else
                    {
                        return false;
                    }
                }
            }

            _unitOfWork.Complete();
            return true;
        }
    }
}
