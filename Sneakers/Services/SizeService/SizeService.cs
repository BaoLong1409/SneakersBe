using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Sneakers.Services.SizeService
{
    public class SizeService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public SizeService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Size> GetAllSizes()
        {
            return _unitOfWork.Size.GetAll().OrderBy(s => s.SizeNumber).ToList();
        }

    }
}
