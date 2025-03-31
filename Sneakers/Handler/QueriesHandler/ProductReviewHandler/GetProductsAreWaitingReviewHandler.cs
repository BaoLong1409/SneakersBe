using Dapper;
using DataAccess.DbContext;
using Domain.ViewModel.ProductReview;
using MediatR;
using Sneakers.Features.Queries.ProductReview;

namespace Sneakers.Handler.QueriesHandler.ProductReviewHandler
{
    public class GetProductsAreWaitingReviewHandler
        : IRequestHandler<GetProductsAreWaittingReview, IEnumerable<ProductsAreWaitingReviewDto>>
    {
        private readonly SneakersDapperContext _context;

        public GetProductsAreWaitingReviewHandler(SneakersDapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductsAreWaitingReviewDto>> Handle(
            GetProductsAreWaittingReview request,
            CancellationToken cancellationToken)
        {
            var query = @"
            SELECT DISTINCT
                od.Id,
                od.Quantity,
                od.PriceAtOrder,
                c.ColorName,
                s.SizeNumber,
                osh.Status,
                p.ProductName,
                pi.ImageUrl
            FROM [Sneakers].[dbo].[OrderDetail] od
            JOIN [Sneakers].[dbo].[Order] o 
                ON o.UserId = @UserId
            JOIN OrderStatusHistory osh 
                ON osh.OrderId = o.Id 
                AND osh.Status = 'Delivered'
            JOIN Product p 
                ON p.Id = od.ProductId
            JOIN ProductImage pi 
                ON pi.ProductId = p.Id 
                AND pi.ColorId = od.ColorId 
                AND pi.IsThumbnail = 1
            JOIN Color c 
                ON c.Id = od.ColorId 
            JOIN Size s 
                ON s.Id = od.SizeId
            WHERE od.Reviewed = 0;
        ";

            using (var connection = _context.CreateConnection())
            {
                var ordersDetail = await connection.QueryAsync<ProductsAreWaitingReviewDto>(
                    query,
                    new { UserId = request.UserId }
                );

                return ordersDetail.ToList();
            }
        }
    }

}
