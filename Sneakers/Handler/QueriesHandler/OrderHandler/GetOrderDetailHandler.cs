using Dapper;
using DataAccess.DbContext;
using Domain.ViewModel.Order;
using MediatR;
using Sneakers.Features.Queries.Order;

namespace Sneakers.Handler.QueriesHandler.OrderHandler
{
    public class GetOrderDetailHandler : IRequestHandler<GetOrderDetails, IEnumerable<OrderDetailDto>>
    {
        private readonly SneakersDapperContext _context;
        public GetOrderDetailHandler(SneakersDapperContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<OrderDetailDto>> Handle(GetOrderDetails request, CancellationToken cancellationToken)
        {
            var query = @"SELECT od.ProductId, od.ColorId, od.SizeId, od.Quantity, od.OrderId, od.PriceAtOrder, p.Name AS ProductName, c.Name AS ColorName, s.SizeNumber, pi.ImageUrl
                        FROM OrderDetail od
                        JOIN Product p ON p.Id = od.ProductId
                        JOIN ProductImage pi ON pi.ProductId = od.ProductId AND pi.ColorId = od.ColorId AND pi.IsThumbnail = 1
                        JOIN Color c ON c.Id = od.ColorId
                        JOIN Size s ON s.Id = od.SizeId
                        WHERE od.OrderId = @OrderId";
            using (var connection = _context.CreateConnection()) {
                var result = await connection.QueryAsync<OrderDetailDto>(query, new {OrderId = request.OrderId});
                return result.ToList();
            }
        }
    }
}
