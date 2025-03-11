using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Sneakers.Services.ShippingService
{
    public class ShippingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ShippingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Shipping> GetAllShippingMethods()
        {
            var shippingMethods = _unitOfWork.Shipping.GetAll();
            return shippingMethods;
        }
    }
}
