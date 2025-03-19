using Dapper;
using DataAccess.DbContext;
using Domain.ViewModel.Order;
using MediatR;
using Sneakers.Features.Queries.Order;

namespace Sneakers.Handler.QueriesHandler.OrderHandler
{
    public class GetAllOrdersHandler : IRequestHandler<GetAllOrders, IEnumerable<AllOrdersDto>>
    {
        private readonly SneakersDapperContext _context;
        public GetAllOrdersHandler(SneakersDapperContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<AllOrdersDto>> Handle(GetAllOrders request, CancellationToken cancellationToken)
        {
            var query = @"SELECT 
                            o.Id AS OrderId, 
                            o.TotalMoney, 
                            p.Name AS FirstProductName, 
                            pi.ImageUrl, 
                            osh.Status AS OrderStatus
                            FROM [dbo].[Order] o
                            JOIN OrderDetail od ON od.OrderId = o.Id
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
                                ORDER BY 
                                    CASE 
                                        WHEN osh.Status = 'Delivered' THEN 1
                                        WHEN osh.Status = 'Delivering' THEN 2
                                        WHEN osh.Status = 'Success' THEN 3
                                        WHEN osh.Status = 'Pending' THEN 4
                                        ELSE 5
                                    END, 
                                    osh.UpdatedAt DESC
                                    ) osh
                            WHERE o.UserId = @UserId;";
            using (var connection = _context.CreateConnection()) {
                var allOrders = connection.QueryAsync<AllOrdersDto>(query, new {UserId =  request.UserId}).Result;
                return allOrders.ToList();
            }
        }
    }
}
