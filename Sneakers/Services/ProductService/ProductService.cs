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

        public Task<IEnumerable<ImageProductDto>> GetImageProductColor(Guid productId, IEnumerable<Color> colors)
        {
            return _unitOfWork.Product.GetImageProductColors(productId, colors);
        }
    }
}
