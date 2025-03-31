using Dapper;
using DataAccess.DbContext;
using Domain.ViewModel.Product;
using MediatR;
using Sneakers.Features.Queries.Products;

namespace Sneakers.Handler.QueriesHandler.ProductsHandler
{
    public class GetAllProductsByCategoryHandler : IRequestHandler<GetAllProductsByCategory, IEnumerable<AllProductsDto>>
    {
        private readonly SneakersDapperContext _context;
        public GetAllProductsByCategoryHandler(SneakersDapperContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<AllProductsDto>> Handle(GetAllProductsByCategory request, CancellationToken cancellationToken)
        {
            var query = @"SELECT DISTINCT p.Id AS ProductId, p.ProductName, p.Price, p.Sale, ct.CategoryName, ct.Brand, c.Id AS ColorId, c.ColorName, i.Id AS ImageId, i.ImageUrl AS ThumbnailUrl
                        FROM Product p 
                        INNER JOIN ProductImage i ON p.Id = i.ProductId AND i.IsThumbnail = 1
                        INNER JOIN ProductQuantity pq ON pq.ProductId = p.Id AND pq.ColorId = i.ColorId
                        INNER JOIN Color c ON c.Id = pq.ColorId
                        INNER JOIN ProductCategory pt ON pt.ProductId = p.Id
                        INNER JOIN Category ct ON ct.id = pt.CategoryId
                        WHERE 
                        (@CategoryName IS NULL OR ct.CategoryName = @CategoryName) 
                        AND (@BrandName IS NULL OR ct.Brand = @BrandName)
                        ORDER BY p.Id, c.Id";
            var productDic = new Dictionary<Guid, AllProductsDto>();
            using (var connection = _context.CreateConnection())
            {
                var allProducts = await connection.QueryAsync<AllProductsDto, AllProductsColorImageDto, AllProductsDto>(query, (product, colorAImage) =>
                {
                    if (!productDic.TryGetValue(product.ProductId, out var productEntry))
                    {
                        productEntry = product;
                        productEntry.ColorsAImages = new List<AllProductsColorImageDto>();
                        productDic.Add(product.ProductId, productEntry);
                    }

                    if (!productEntry.ColorsAImages.Any(c => c.ColorId == colorAImage.ColorId))
                    {
                        productEntry.ColorsAImages.Add(colorAImage);
                    }

                    return productEntry;
                },
                splitOn: "ColorId",
                param: new { CategoryName = request.CategoryName, BrandName = request.BrandName }
                );
                return productDic.Values.ToList();
            }
        }
    }
}
