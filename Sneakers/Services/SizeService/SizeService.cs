using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ViewModel.Product;
using MediatR;
using Sneakers.Features.Queries.Size;

namespace Sneakers.Services.SizeService
{
    public class SizeService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        public SizeService(IMapper mapper, IUnitOfWork unitOfWork, IMediator mediator)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public IEnumerable<Size> GetAllSizes()
        {
            return _unitOfWork.Size.GetAll().OrderBy(s => s.SizeNumber).ToList();
        }

        public async Task<IEnumerable<UploadSizeRequest>> GetAvailableSizes(Guid productId, string colorName)
        {
            return await _mediator.Send(new GetAvailableSizes(productId, colorName));
        }

    }
}
