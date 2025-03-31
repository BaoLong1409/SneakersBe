using Domain.Entities;
using Domain.ViewModel.Product;
using MediatR;

namespace Sneakers.Features.Queries.Size
{
    public record GetAvailableSizes (Guid ProductId, string ColorName) : IRequest<IEnumerable<UploadSizeRequest>>
    {
    }
}
