using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Dapper;
using DataAccess.DbContext;
using Domain.Enum;
using MediatR;
using Sneakers.Features.Command.Product;

namespace Sneakers.Handler.CommandHandler.ProductsHandler
{
    public class UploadNewProductHandler : IRequestHandler<UploadNewProduct, EnumProduct>
    {
        private readonly SneakersDapperContext _context;
        private readonly IConfiguration _configuration;

        public UploadNewProductHandler(SneakersDapperContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<EnumProduct> Handle(UploadNewProduct request, CancellationToken cancellationToken)
        {
            var existProductQuery = @"SELECT p.Id FROM Product p JOIN ProductCategory pc ON pc.ProductId = p.Id JOIN Category c ON c.Brand = @Brand WHERE p.Name = @Name";
            var existProductColorQuery = @"SELECT c.Id FROM Color c JOIN ProductImage pi ON pi.ColorId = c.Id JOIN Product p ON pi.ProductId = p.Id WHERE c.Name = @ColorName AND p.Id = @ProductId";
            var uploadProductCommand = @"INSERT INTO Product (Name, CreatedAt, UpdatedAt, Price, Sale) OUTPUT INSERTED.Id VALUES  (@ProductName, GETDATE(), GETDATE(), @Price, @Sale)";
            var categoryQuery = @"SELECT DISTINCT c.Id AS CategoryId FROM Category c WHERE c.Name IN @Name AND c.Brand = @Brand";
            var uploadProductCategoryQuery = @"INSERT INTO ProductCategory (ProductId, CategoryId) VALUES (@ProductId, @CategoryId)";
            var uploadProductImageCommand = @"INSERT INTO ProductImage (ImageUrl, IsThumbnail, ProductId, ColorId) VALUES (@ImageUrl, @IsThumbnail, @ProductId, @ColorId)";
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
                        await cloudinary.CreateFolderAsync($"ProductImage/{request.Request.Brand}/{request.Request.ProductName}/{request.Request.Color.Name}");

                        var productId = await connection.QueryFirstOrDefaultAsync<Guid>(existProductQuery, new
                        {
                            Name = request.Request.ProductName,
                            Brand = request.Request.Brand
                        }, transaction: transaction);


                        if (productId != Guid.Empty)
                        {

                            var existProductColorId = await connection.QueryFirstOrDefaultAsync<Guid>(existProductColorQuery, new
                            {
                                ColorName = request.Request.Color.Name,
                                ProductId = productId
                            }, transaction: transaction);

                            if (existProductColorId != Guid.Empty)
                            {
                                return EnumProduct.ProductExist;
                            }
                            else
                            {
                                for (int i = 0; i < request.Request.ProductImages.Count(); i++)
                                {
                                    byte[] imageBytes = Convert.FromBase64String(request.Request.ProductImages[i].Base64Data.Split(',')[1]);
                                    using var stream = new MemoryStream(imageBytes);
                                    var uploadParams = new ImageUploadParams()
                                    {
                                        File = new FileDescription(request.Request.ProductImages[i].FileName, stream),
                                        AssetFolder = $"ProductImage/{request.Request.Brand}/{request.Request.ProductName}/{request.Request.Color.Name}"
                                    };
                                    var result = cloudinary.Upload(uploadParams);

                                    await connection.ExecuteAsync(uploadProductImageCommand, new
                                    {
                                        ImageUrl = result.Url.AbsoluteUri,
                                        IsThumbnail = request.Request.OrdinalImageThumbnail == i ? 1 : 0,
                                        ProductId = productId,
                                        ColorId = request.Request.Color.Id
                                    }, transaction: transaction);
                                }

                                foreach (var size in request.Request.SizesQuantity)
                                {
                                    await connection.ExecuteAsync(uploadProductQuantityCommand, new
                                    {
                                        SizeId = size.Id,
                                        ProductId = productId,
                                        ColorId = request.Request.Color.Id,
                                        StockQuantity = size.Quantity
                                    }, transaction: transaction);
                                }
                                transaction.Commit();
                                return EnumProduct.UploadProductSuccessfully;
                            }

                        }

                        productId = await connection.ExecuteScalarAsync<Guid>(uploadProductCommand, new
                        {
                            ProductName = request.Request.ProductName,
                            Price = request.Request.Price,
                            Sale = request.Request.Sale
                        }, transaction: transaction);

                        var categoryIds = await connection.QueryAsync<Guid>(categoryQuery, new
                        {
                            Name = request.Request.CategoryName,
                            Brand = request.Request.Brand
                        }, transaction: transaction);

                        foreach (var categoryId in categoryIds)
                        {
                            await connection.ExecuteAsync(uploadProductCategoryQuery, new
                            {
                                ProductId = productId,
                                CategoryId = categoryId
                            }, transaction: transaction);
                        }

                        for (int i = 0; i < request.Request.ProductImages.Count(); i++)
                        {
                            byte[] imageBytes = Convert.FromBase64String(request.Request.ProductImages[i].Base64Data.Split(',')[1]);
                            using var stream = new MemoryStream(imageBytes);
                            var uploadParams = new ImageUploadParams()
                            {
                                File = new FileDescription(request.Request.ProductImages[i].FileName, stream),
                                AssetFolder = $"ProductImage/{request.Request.Brand}/{request.Request.ProductName}/{request.Request.Color.Name}"
                            };
                            var result = cloudinary.Upload(uploadParams);

                            await connection.ExecuteAsync(uploadProductImageCommand, new
                            {
                                ImageUrl = result.Url.AbsoluteUri,
                                IsThumbnail = request.Request.OrdinalImageThumbnail == i ? 1 : 0,
                                ProductId = productId,
                                ColorId = request.Request.Color.Id
                            }, transaction: transaction);
                        }

                        foreach (var size in request.Request.SizesQuantity)
                        {
                            await connection.ExecuteAsync(uploadProductQuantityCommand, new
                            {
                                SizeId = size.Id,
                                ProductId = productId,
                                ColorId = request.Request.Color.Id,
                                StockQuantity = size.Quantity
                            }, transaction: transaction);
                        }
                        transaction.Commit();
                        return EnumProduct.UploadProductSuccessfully;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return EnumProduct.UploadProductFail;
                    }
                }

            }

        }
    }
}
