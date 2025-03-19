using Dapper;
using DataAccess.DbContext;
using Domain.ViewModel.Order;
using MediatR;
using Sneakers.Features.Queries.Order;

namespace Sneakers.Handler.QueriesHandler.OrderHandler
{
    public class GetOrderInfoHandler : IRequestHandler<GetOrderInfo, OrderInfoDto>
    {
        private readonly SneakersDapperContext _context;
        public GetOrderInfoHandler(SneakersDapperContext context)
        {
            _context = context;
        }
        public async Task<OrderInfoDto> Handle(GetOrderInfo request, CancellationToken cancellationToken)
        {
            var query = @"SELECT o.Id, o.OrderDate, o.ShippingDate, o.TotalMoney, o.FullName, o.PhoneNumber, o.ShippingAddress, s.MinimumDeliveredTime, s.MaximumDeliveredTime, s.Name AS ShippingName, s.Price AS ShippingPrice, p.Name AS PaymentName, osh.Id AS StatusId, osh.Status, osh.UpdatedAt, osh.Note AS StatusNote
                            FROM [Sneakers].[dbo].[Order] o
                            JOIN Shipping s ON s.Id = o.ShippingId
                            JOIN Payment p ON p.Id = o.PaymentId
                            JOIN OrderStatusHistory osh ON osh.OrderId = o.Id
                            WHERE o.Id = @OrderId";
            var orderDic = new Dictionary<Guid, OrderInfoDto>();
            using (var connection = _context.CreateConnection())
            {
                await connection.QueryAsync<OrderInfoDto, OrderStatusHistoryDto, OrderInfoDto>(query, (order, orderStatus) =>
                {
                    if (!orderDic.TryGetValue(order.Id, out var orderEntry))
                    {
                        orderEntry = order;
                        orderEntry.StatusHistories = new List<OrderStatusHistoryDto>();
                        orderDic.Add(order.Id, orderEntry);
                    }

                    orderEntry.StatusHistories.Add(orderStatus);
                    return orderEntry;
                },
                param: new {OrderId = request.OrderId},
                splitOn: "StatusId"
                );
                return orderDic.Values.FirstOrDefault();
            }
        }
    }
}
