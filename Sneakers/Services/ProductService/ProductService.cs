using AutoMapper;
using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces;
using Domain.ViewModel.Product;
using MediatR;
using Sneakers.Features.Command.Product;
using Sneakers.Features.Queries.Products;
using System.Diagnostics;

namespace Sneakers.Services.ProductService
{
    public class ProductService
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        public ProductService(IMapper mapper, IUnitOfWork unitOfWork, IMediator mediator)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<IEnumerable<ImageProductDto>> GetImageProductColor(Guid productId, IEnumerable<Color> colors)
        {
            return await _unitOfWork.Product.GetImageProductColors(productId, colors);
        }

        public async Task<IEnumerable<AvailableProductsDto>> GetAvailableProducts(Guid productId, Guid colorId)
        {
            return await _unitOfWork.ProductQuantity.GetAvailableProducts(productId, colorId);
        }

        public async Task<IEnumerable<AllProductsDto>> GetProductsWithCondition(int[] priceFilter)
        {
            return await _mediator.Send(new GetAllProductsWithCondition(priceFilter));
        }

        public async Task<IEnumerable<AllProductsDto>> GetAllProductsByCategory(GetProductsByCategoryReq req)
        {
            return await _mediator.Send(new GetAllProductsByCategory(req.CategoryName, req.BrandName));
        }

        public async Task<EnumProduct> UploadNewProduct(UploadNewProductRequest request)
        {
            return await _mediator.Send(new UploadNewProduct(request));
        }

        public async Task<EnumProduct> UpdateProduct(UpdateProductRequest request)
        {
            return await _mediator.Send(new UpdateProduct(request));
        }

        public async Task<EnumProduct> DeleteProduct(Guid productId)
        {
            return await _mediator.Send(new DeleteProduct(productId));
        }
    }
}
