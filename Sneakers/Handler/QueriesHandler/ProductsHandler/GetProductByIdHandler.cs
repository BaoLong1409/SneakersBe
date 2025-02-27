using Dapper;
using DataAccess.DbContext;
using Domain.Entities;
using Domain.ViewModel;
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
            var query = @"WITH ColorInfor AS
                        (SELECT c.Id FROM Color c WHERE c.Name = @colorName)
                        SELECT p.*, pi.Id, pi.ImageUrl, pi.IsThumbnail, pi.ProductId FROM Product p
                        JOIN ProductImage pi ON p.Id = pi.ProductId
                        JOIN ColorInfor cf ON cf.Id = pi.ColorId
                        WHERE p.Id = @productId";
            var productDic = new Dictionary<Guid, DetailProductDto>();
            using (var connection = _context.CreateConnection()) {
                 await connection.QueryAsync<DetailProductDto, ProductImageDto, DetailProductDto>(query, (product, image) =>
                {
                    if (!productDic.TryGetValue(product.Id, out var productEntry))
                    {
                        productEntry = product;
                        productEntry.ProductImages = new List<ProductImageDto>();
                        productDic.Add(product.Id, productEntry);
                    }
                    productEntry.ProductImages.Add(image);

                    return productEntry;
                },
                param : new {productId = request.ProductId,colorName = request.ColorName },
                splitOn: "Id"
                );
                return productDic.Values.FirstOrDefault();
            }
        }
    }
}
