using Dapper;
using DataAccess.DbContext;
using Domain.ViewModel.Order;
using MediatR;
using Sneakers.Features.Queries.Order;

namespace Sneakers.Handler.QueriesHandler.OrderHandler
{
    public class GetOrdersHandler : IRequestHandler<GetOrders, IEnumerable<OrdersDto>>
    {
        private readonly SneakersDapperContext _context;
        public GetOrdersHandler(SneakersDapperContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<OrdersDto>> Handle(GetOrders request, CancellationToken cancellationToken)
        {
            var query = @"SELECT 
                            o.Id AS OrderId, 
                            o.TotalMoney, 
                            o.OrderDate,
                            p.ProductName AS FirstProductName, 
                            pi.ImageUrl, 
                            osh.Status AS OrderStatus
                            FROM [dbo].[Order] o
                            OUTER APPLY (SELECT TOP 1 od.ProductId, od.ColorId FROM OrderDetail od WHERE od.OrderId = o.Id) od
                            JOIN Product p ON p.Id = od.ProductId
                            OUTER APPLY (
                                SELECT TOP 1 pi.ImageUrl 
                                FROM ProductImage pi
                                WHERE pi.ProductId = od.ProductId AND pi.ColorId = od.ColorId
                                ) pi
                            OUTER APPLY (
                                SELECT TOP 1 osh.*
                                FROM OrderStatusHistory osh
                                WHERE osh.OrderId = o.Id
                                ORDER BY osh.UpdatedAt DESC
                                    ) osh";
            if (request.UserId != null)
            {
                query += @" WHERE o.UserId = @UserId ORDER BY o.OrderDate";
            } else
            {
                query += @" WHERE osh.Status = @Status ORDER BY o.OrderDate OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";
            }
            using (var connection = _context.CreateConnection()) {
                var allOrders = connection.QueryAsync<OrdersDto>(query, new {UserId =  request.UserId, Offset = request.Offset, Limit = request.Limit, Status = request.Status}).Result;
                return allOrders.ToList();
            }
        }
    }
}
