using AutoMapper;
using Domain.Interfaces;
using Domain.ViewModel.Category;
using MediatR;

namespace Sneakers.Services.CategoryService
{
    public class CategoryService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        public CategoryService(IMapper mapper, IUnitOfWork unitOfWork, IMediator mediator)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public IEnumerable<CategoryDto> GetAllCategory()
        {
            var category = _unitOfWork.Category.GetAll();
            return _mapper.Map<IEnumerable<CategoryDto>>(category);
        }
    }
}
