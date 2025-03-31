using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Dapper;
using DataAccess.DbContext;
using Domain.Enum;
using MediatR;
using Sneakers.Features.Command.Product;

namespace Sneakers.Handler.CommandHandler.ProductsHandler
{
    public class UpdateProductHandler : IRequestHandler<UpdateProduct, EnumProduct>
    {
        private readonly SneakersDapperContext _context;
        private readonly IConfiguration _configuration;
        public UpdateProductHandler(SneakersDapperContext context, IConfiguration configuration)
        {
            _context = context;   
            _configuration = configuration;
        }
        public async Task<EnumProduct> Handle(UpdateProduct request, CancellationToken cancellationToken)
        {
            var updateProductCommand = @"UPDATE Product SET ProductName = @ProductName, UpdatedAt = GETDATE(), Price = @Price, Sale = @Sale WHERE Id = @ProductId";
            var categoryQuery = @"SELECT DISTINCT c.Id AS CategoryId FROM Category c WHERE c.CategoryName IN @Name AND c.Brand = @Brand";
            var deleteProductCategoryCommand = @"DELETE FROM ProductCategory WHERE ProductId = @ProductId";
            var updateProductCategoryCommand = @"INSERT INTO ProductCategory (ProductId, CategoryId) VALUES (@ProductId, @CategoryId)";
            var deleteProductImageCommand = @"DELETE FROM ProductImage WHERE ProductId = @ProductId AND ColorId = @ColorId";
            var uploadProductImageCommand = @"INSERT INTO ProductImage (ImageUrl, IsThumbnail, ProductId, ColorId) VALUES (@ImageUrl, @IsThumbnail, @ProductId, @ColorId)";
            var deleteProductQuantityCommand = @"DELETE FROM ProductQuantity WHERE ProductId = @ProductId AND ColorId = @ColorId";
            var uploadProductQuantityCommand = @"INSERT INTO ProductQuantity (SizeId, ProductId, ColorId, StockQuantity) VALUES (@SizeId, @ProductId, @ColorId, @StockQuantity)";


            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var cloudinaryApi = _configuration["CloudinaryApi:Url"];
                        Cloudinary cloudinary = new Cloudinary(cloudinaryApi);
                        cloudinary.Api.Secure = true;
                        await cloudinary.CreateFolderAsync($"ProductImage/{request.Request.Brand}/{request.Request.ProductName}");
                        await cloudinary.CreateFolderAsync($"ProductImage/{request.Request.Brand}/{request.Request.ProductName}/{request.Request.Color.ColorName}");

                        await connection.ExecuteAsync(updateProductCommand, new
                        {
                            ProductName = request.Request.ProductName,
                            Price = request.Request.Price,
                            Sale = request.Request.Sale,
                            ProductId = request.Request.ProductId
                        }, transaction: transaction);

                        await connection.ExecuteAsync(deleteProductCategoryCommand, new
                        {
                            ProductId = request.Request.ProductId
                        }, transaction: transaction);

                        var categoryIds = await connection.QueryAsync<Guid>(categoryQuery, new
                        {
                            Name = request.Request.CategoryName,
                            Brand = request.Request.Brand
                        }, transaction: transaction);

                        foreach (var categoryId in categoryIds)
                        {
                            await connection.ExecuteAsync(updateProductCategoryCommand, new
                            {
                                ProductId = request.Request.ProductId,
                                CategoryId = categoryId
                            }, transaction: transaction);
                        }

                        await connection.ExecuteAsync(deleteProductImageCommand, new
                        {
                            ProductId = request.Request.ProductId,
                            ColorId = request.Request.Color.Id
                        }, transaction: transaction);

                        for (int i = 0; i < request.Request.ProductImages.Count(); i++)
                        {
                            byte[] imageBytes = Convert.FromBase64String(request.Request.ProductImages[i].Base64Data.Split(',')[1]);
                            using var stream = new MemoryStream(imageBytes);
                            var uploadParams = new ImageUploadParams()
                            {
                                File = new FileDescription(request.Request.ProductImages[i].FileName, stream),
                                AssetFolder = $"ProductImage/{request.Request.Brand}/{request.Request.ProductName}/{request.Request.Color.ColorName}"
                            };
                            var result = cloudinary.Upload(uploadParams);

                            await connection.ExecuteAsync(uploadProductImageCommand, new
                            {
                                ImageUrl = result.Url.AbsoluteUri,
                                IsThumbnail = request.Request.OrdinalImageThumbnail == i ? 1 : 0,
                                ProductId = request.Request.ProductId,
                                ColorId = request.Request.Color.Id
                            }, transaction: transaction);
                        }

                        await connection.ExecuteAsync(deleteProductQuantityCommand, new
                        {
                            ProductId = request.Request.ProductId,
                            ColorId = request.Request.Color.Id
                        }, transaction: transaction);

                        foreach (var size in request.Request.SizesQuantity)
                        {
                            await connection.ExecuteAsync(uploadProductQuantityCommand, new
                            {
                                SizeId = size.Id,
                                ProductId = request.Request.ProductId,
                                ColorId = request.Request.Color.Id,
                                StockQuantity = size.Quantity
                            }, transaction: transaction);
                        }
                        transaction.Commit();
                        return EnumProduct.UpdateProductSuccessfully;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return EnumProduct.UpdateProductFail;
                    }
                }

            }
        }
    }
}
