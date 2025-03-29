using AutoMapper;
using DataAccess.UnitOfWork;
using Domain.Entities;
using Domain.Interfaces;

namespace Sneakers.Services.ColorService
{
    public class ColorService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public ColorService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Color?>> GetAllProductColors(Guid productId)
        {
            return await _unitOfWork.Color.GetProductColors(productId);
        }

        public async Task<Color?> GetColorFromColorName(String colorName)
        {
            return await _unitOfWork.Color.GetFirstOrDefaultAsync(c => c.Name == colorName);
        }

        public IEnumerable<Color>? GetAllColors()
        {
            return _unitOfWork.Color.GetAll();
        }
    }
}
