using Dapper;
using DataAccess.DbContext;
using Domain.Enum;
using MediatR;
using Sneakers.Features.Command.Product;

namespace Sneakers.Handler.CommandHandler.ProductsHandler
{
    public class DeleteProductHandler : IRequestHandler<DeleteProduct, EnumProduct>
    {
        private readonly SneakersDapperContext _context;
        public DeleteProductHandler(SneakersDapperContext context)
        {
            _context = context;
        }
        public async Task<EnumProduct> Handle(DeleteProduct request, CancellationToken cancellationToken)
        {
            var query = @"
                          BEGIN TRANSACTION;
                          WITH OrderDetailId AS (
                            SELECT c.Id FROM OrderDetail c WHERE c.ProductId = @ProductId
                          ),
                          ProductReviewId AS (
                            SELECT pr.Id FROM ProductReview pr WHERE pr.ProductId = @ProductId
                          )

                          DELETE pri FROM ProductReviewImage pri
                          JOIN ProductReviewId pr ON pri.ProductReviewId = pr.Id;

                          DELETE pr FROM ProductReview pr
                          JOIN OrderDetailId o ON pr.OrderDetailId = o.Id;

                          DELETE FROM ProductCart WHERE ProductId = @ProductId;
                          DELETE FROM ProductQuantity WHERE ProductId = @ProductId;
                          DELETE FROM ProductImage WHERE ProductId = @ProductId;
                          DELETE FROM ProductTranslation WHERE ProductId = @ProductId;
                          DELETE FROM ProductCategory WHERE ProductId = @ProductId;
                          DELETE FROM Product WHERE Id = @ProductId;
                          COMMIT TRANSACTION";

            using (var connection = _context.CreateConnection())
            {
                try
                {
                    var rowEffected = await connection.ExecuteAsync(query, new
                    {
                        ProductId = request.ProductId
                    });

                    if (rowEffected == 0)
                    {
                        return EnumProduct.DeleteProductFail;
                    }

                    return EnumProduct.DeleteProductSuccessfully;
                } catch (Exception ex)
                {
                    return EnumProduct.DeleteProductFail;
                }

            }
        }
    }
}
