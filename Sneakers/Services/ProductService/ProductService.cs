using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ViewModel;

namespace Sneakers.Services.ProductService
{
    public class ProductService
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ImageProductDto>> GetImageProductColor(Guid productId, IEnumerable<Color> colors)
        {
            return await _unitOfWork.Product.GetImageProductColors(productId, colors);
        }

        public async Task<IEnumerable<AvailableProductsDto>> GetAvailableProducts(Guid productId, Guid colorId)
        {
            return await _unitOfWork.ProductQuantity.GetAvailableProducts(productId, colorId);
        }
    }
}
