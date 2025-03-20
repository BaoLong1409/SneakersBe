using Dapper;
using DataAccess.DbContext;
using Domain.ViewModel.Product;
using MediatR;
using Sneakers.Features.Queries.Products;

namespace Sneakers.Handler.QueriesHandler.ProductsHandler
{
    public class GetAllProductsWithConditionHandler : IRequestHandler<GetAllProductsWithCondition, IEnumerable<AllProductsDto>>
    {
        private readonly SneakersDapperContext _context;
        public GetAllProductsWithConditionHandler(SneakersDapperContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<AllProductsDto>> Handle(GetAllProductsWithCondition request, CancellationToken cancellationToken)
        {
            var query = @"SELECT DISTINCT p.Id, p.Name, p.Price, p.Sale, c.Id AS ColorId, c.Name AS ColorName, i.Id AS ImageId, i.ImageUrl AS ThumbnailUrl
                        FROM Product p 
                        INNER JOIN ProductImage i ON p.Id = i.ProductId AND i.IsThumbnail = 1
                        INNER JOIN ProductQuantity pq ON pq.ProductId = p.Id AND pq.ColorId = i.ColorId
                        INNER JOIN Color c ON c.Id = pq.ColorId
                        WHERE p.Price BETWEEN @MinValue AND @MaxValue
                        ORDER BY p.Id, c.Id";
            var productDic = new Dictionary<Guid, AllProductsDto>();
            using (var connection = _context.CreateConnection())
            {
                var allProducts = await connection.QueryAsync<AllProductsDto, AllProductsColorImageDto, AllProductsDto>(query, (product, colorAImage) =>
                {
                    if (!productDic.TryGetValue(product.Id, out var productEntry))
                    {
                        productEntry = product;
                        productEntry.ColorsAImages = new List<AllProductsColorImageDto>();
                        productDic.Add(product.Id, productEntry);
                    }

                    if (!productEntry.ColorsAImages.Any(c => c.ColorId == colorAImage.ColorId))
                    {
                        productEntry.ColorsAImages.Add(colorAImage);
                    }

                    return productEntry;
                },
                splitOn: "ColorId",
                param: new { MinValue = request.MinValue, MaxValue = request.MaxValue }
                );
                return productDic.Values.ToList();
            }
        }
    }
}
