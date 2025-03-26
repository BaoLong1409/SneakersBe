using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces;
using Domain.ViewModel.ProductReview;
using MediatR;
using Sneakers.Features.Queries.ProductReview;

namespace Sneakers.Services.ProductService
{
    public class ProductReviewService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        public ProductReviewService(IMapper mapper, IUnitOfWork unitOfWork, IMediator mediator, IConfiguration configuration)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _configuration = configuration;
        }

        public async Task<EnumProductReview> PostProductReview(ProductReviewRequest request)
        {
            var orderDetail = await _unitOfWork.OrderDetail.GetFirstOrDefaultAsync(od => od.Id == request.OrderDetailId);
            if (orderDetail == null)
            {
                return EnumProductReview.OrderDetailNotExist;
            }

            orderDetail.Reviewed = 1;
            var order = await _unitOfWork.Order.GetFirstOrDefaultAsync(o => o.Id == orderDetail.OrderId);
            if (order == null)
            {
                return EnumProductReview.OrderNotExist;
            }

            var orderStatusHistory = await _unitOfWork.OrderStatusHistory.GetFirstOrDefaultAsync(osh => osh.OrderId == order.Id);
            if (orderStatusHistory == null)
            {
                return EnumProductReview.OrderNotExist;
            }

            var productReview = _mapper.Map<ProductReview>(request);
            productReview.Id = Guid.NewGuid();
            _unitOfWork.ProductReview.Add(productReview);

            List<ProductReviewImage> productReviewImageEntities = new List<ProductReviewImage>();

            if (request.Image != null && request.Image.Count() <= 5)
            {
                foreach (var image in request.Image)
                {
                    var cloudinaryApi = _configuration["CloudinaryApi:Url"];
                    Cloudinary cloudinary = new Cloudinary(cloudinaryApi);
                    cloudinary.Api.Secure = true;

                    byte[] imageBytes = Convert.FromBase64String(image.Base64Data.Split(',')[1]);
                    using var stream = new MemoryStream(imageBytes);

                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(image.FileName ,stream),
                        AssetFolder = "ProductReview"
                    };

                    var result = cloudinary.Upload(uploadParams);

                    productReviewImageEntities.Add(new ProductReviewImage
                    {
                        ProductReviewId = productReview.Id,
                        ImageUrl = result.SecureUrl.ToString()
                    });
                }

                _unitOfWork.ProductReviewImage.AddRange(productReviewImageEntities);
                _unitOfWork.Complete();
                return EnumProductReview.ReviewProductSuccessfully;
            }

            return EnumProductReview.ReviewProductFailed;
        }

        public async Task<IEnumerable<ProductsAreWaitingReviewDto>> GetProductsWaittingReview(Guid userId)
        {
            return await _mediator.Send(new GetProductsAreWaittingReview(userId));
        }

        public async Task<IEnumerable<ProductReviewDto>> GetCommentsOfProducts(GetCommentsOfProductRequest req)
        {
            return await _mediator.Send(new GetCommentOfProduct(req.ProductId, req.ColorName));
        }
    }
}
