using Domain.Enum;
using Domain.ViewModel.Product;
using MediatR;

namespace Sneakers.Features.Command.Product
{
    public  record UpdateProduct (UpdateProductRequest Request) : IRequest<EnumProduct>
    {
    }
}
