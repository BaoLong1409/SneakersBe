using Dapper;
using DataAccess.DbContext;
using Domain.Entities;
using Domain.ViewModel.Category;
using Domain.ViewModel.Product;
using MediatR;
using Sneakers.Features.Queries.Products;

namespace Sneakers.Handler.QueriesHandler.ProductsHandler
{
    public class GetProductByIdHandler : IRequestHandler<GetProductById, DetailProductDto>
    {
        private SneakersDapperContext _context;
        public GetProductByIdHandler(SneakersDapperContext context)
        {
            _context = context;
        }
        public async Task<DetailProductDto> Handle(GetProductById request, CancellationToken cancellationToken)
        {
            var query = @"SELECT DISTINCT p.Id AS ProductId, p.ProductName, p.Sale, p.Price, c.Id AS ColorId, c.ColorName, ct.Id AS CategoryId, ct.CategoryName, ct.Brand, pi.Id AS ProductImageId, pi.ImageUrl, pi.IsThumbnail
	                    FROM Product p
	                    JOIN ProductQuantity pq ON pq.ProductId = p.Id
	                    JOIN Color c ON c.Id = pq.ColorId
	                    JOIN ProductCategory pc ON pc.ProductId = p.Id
	                    JOIN Category ct ON ct.Id = pc.CategoryId
	                    JOIN ProductImage pi ON pi.ProductId = p.Id AND pi.ColorId = c.Id
	                    WHERE p.Id = @ProductId AND c.ColorName = @ColorName";
            var productDic = new Dictionary<Guid, DetailProductDto>();
            using (var connection = _context.CreateConnection())
            {
                await connection.QueryAsync<DetailProductDto, CategoryDto, ProductImageDto, DetailProductDto>(query, (product, category, image) =>
                {
                    if (!productDic.TryGetValue(product.ProductId, out var productEntry))
                    {
                        productEntry = product;
                        productEntry.Categories = new List<CategoryDto>();
                        productEntry.ProductImages = new List<ProductImageDto>();
                        productDic.Add(product.ProductId, productEntry);
                    }

                    if (!productEntry.Categories.Any(c => c.CategoryId == category.CategoryId))
                    {
                        productEntry.Categories.Add(category);
                    }

                    if (!productEntry.ProductImages.Any(img => img.ProductImageId == image.ProductImageId))
                    {
                        productEntry.ProductImages.Add(image);
                    }

                    return productEntry;
                },
               param: new { ProductId = request.ProductId, ColorName = request.ColorName },
               splitOn: "CategoryId,ProductImageId"
               );
                return productDic.Values.FirstOrDefault();
            }
        }
    }
}
