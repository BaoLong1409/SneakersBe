using AutoMapper;
using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces;
using Domain.ViewModel.User;

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
        public async Task<(IEnumerable<ShippingInfoDto>, EnumShippingInfo)> AddShippingInfo(ShippingInfoDto shippingInfoDto)
        {
            var shippingInfo = _mapper.Map<ShippingInfor>(shippingInfoDto);
            _unitOfWork.ShippingInfo.Add(shippingInfo);
            _unitOfWork.Complete();
            var newListShippingInfo = await GetShippingInfo(shippingInfoDto.UserId);
            return (newListShippingInfo, EnumShippingInfo.AddSuccessfully);
        }

        public async Task<IEnumerable<ShippingInfoDto>> GetShippingInfo(Guid userId)
        {
            var shippingInfoDto = await _unitOfWork.ShippingInfo.FindAsync(x => x.UserId == userId);
            var shippingInfo = _mapper.Map<IEnumerable<ShippingInfoDto>>(shippingInfoDto);
            return shippingInfo;
        }

        public async Task< (IEnumerable<ShippingInfoDto> , EnumShippingInfo)> UpdateShippingInfo(ShippingInfoDto shippingInfoDto)
        {
            var shippingExist = await _unitOfWork.ShippingInfo.GetFirstOrDefaultAsync(x => x.Id == shippingInfoDto.Id);
            if (shippingExist == null)
            {
                return (Enumerable.Empty<ShippingInfoDto>(), EnumShippingInfo.NotExist);
            }
            _mapper.Map(shippingInfoDto, shippingExist);
            _unitOfWork.Complete();
            var newListShippingInfo = await GetShippingInfo(shippingInfoDto.UserId);
            return (newListShippingInfo, EnumShippingInfo.UpdateSuccessfully);
        }

        public async Task<(IEnumerable<ShippingInfoDto>, EnumShippingInfo)> DeleteShippingInfo(Guid shippingInfoId)
        {
            var shippingExist = await _unitOfWork.ShippingInfo.GetFirstOrDefaultAsync(x => x.Id == shippingInfoId);
            if (shippingExist == null)
            {
                return (Enumerable.Empty<ShippingInfoDto>(), EnumShippingInfo.NotExist);
            }
            var userId = shippingExist.UserId;
            _unitOfWork.ShippingInfo.Remove(shippingExist);
            _unitOfWork.Complete();
            var newListShippingInfo = await GetShippingInfo(userId);
            return (newListShippingInfo, EnumShippingInfo.DeleteSuccessfully);
        }

    }
}
