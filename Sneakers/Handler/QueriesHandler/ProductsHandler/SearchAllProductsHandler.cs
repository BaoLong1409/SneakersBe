using Dapper;
using DataAccess.DbContext;
using Domain.ViewModel.Product;
using MediatR;
using Sneakers.Features.Queries.Products;

namespace Sneakers.Handler.QueriesHandler.ProductsHandler
{
    public class SearchAllProductsHandler : IRequestHandler<SearchAllProducts, IEnumerable<AllProductsDto>>
    {
        private readonly SneakersDapperContext _context;

        public SearchAllProductsHandler(SneakersDapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AllProductsDto>> Handle(SearchAllProducts request, CancellationToken cancellationToken)
        {
            var keyword = $"%{request.Keyword}%";

            const string query = @"
                WITH RankedProducts AS (
                    SELECT 
                        p.Id AS ProductId, p.ProductName, p.Price, p.Sale, 
                        ct.CategoryName, ct.Brand,
                        c.Id AS ColorId, c.ColorName, 
                        i.Id AS ImageId, i.ImageUrl AS ThumbnailUrl,
                        ROW_NUMBER() OVER (PARTITION BY p.ProductName ORDER BY p.Id, c.Id) AS rn
                    FROM Product p 
                    INNER JOIN ProductImage i ON p.Id = i.ProductId AND i.IsThumbnail = 1
                    INNER JOIN ProductQuantity pq ON pq.ProductId = p.Id AND pq.ColorId = i.ColorId
                    INNER JOIN Color c ON c.Id = pq.ColorId
                    INNER JOIN ProductCategory pc ON pc.ProductId = p.Id
                    INNER JOIN Category ct ON ct.Id = pc.CategoryId
                    WHERE p.ProductName LIKE @Keyword OR ct.CategoryName LIKE @Keyword OR ct.Brand LIKE @Keyword
                )
                SELECT * FROM RankedProducts WHERE rn = 1;
            ";

            using var connection = _context.CreateConnection();
            var productDic = new Dictionary<Guid, AllProductsDto>();

            var allProducts = await connection.QueryAsync<AllProductsDto, AllProductsColorImageDto, AllProductsDto>(
                query, 
                (product, colorImage) =>
                {
                    if (!productDic.TryGetValue(product.ProductId, out var productEntry))
                    {
                        productEntry = product;
                        productEntry.ColorsAImages = new List<AllProductsColorImageDto>();
                        productDic[product.ProductId] = productEntry;
                    }

                    if (!productEntry.ColorsAImages.Any(c => c.ColorId != colorImage.ColorId))
                        productEntry.ColorsAImages.Add(colorImage);

                    return productEntry;
                },
                param: new { Keyword = keyword },
                splitOn: "ColorId"
            );

            return productDic.Values;
        }
    }
}
