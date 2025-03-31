using Dapper;
using DataAccess.DbContext;
using Domain.Entities;
using Domain.ViewModel.Product;
using MediatR;
using Sneakers.Features.Queries.Size;

namespace Sneakers.Handler.QueriesHandler.SizeHandler
{
    public class GetAvailableSizesHandler : IRequestHandler<GetAvailableSizes, IEnumerable<UploadSizeRequest>>
    {
        private readonly SneakersDapperContext _context;
        public GetAvailableSizesHandler(SneakersDapperContext context)
        {
           _context = context;
        }
        public async Task<IEnumerable<UploadSizeRequest>> Handle(GetAvailableSizes request, CancellationToken cancellationToken)
        {
            var getAvailableSizesQuery = @"SELECT DISTINCT
            s.Id, 
            s.SizeNumber, 
            COALESCE(pq.StockQuantity, 0) AS Quantity
            FROM Size s
            JOIN Color c ON c.ColorName = @ColorName
            LEFT JOIN [Sneakers].[dbo].[ProductQuantity] pq 
                ON s.Id = pq.SizeId 
                AND pq.ProductId = @ProductId
                AND pq.ColorId = c.Id
                ORDER BY s.SizeNumber;";
            
            using (var connection = _context.CreateConnection())
            {
                var availableSizes = await connection.QueryAsync<UploadSizeRequest>(getAvailableSizesQuery, new
                {
                    ProductId = request.ProductId,
                    ColorName = request.ColorName
                });
                return availableSizes.ToList();
            }
        }
    }
}
