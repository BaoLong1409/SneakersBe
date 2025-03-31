using Domain.Enum;
using MediatR;
using Microsoft.Identity.Client;

namespace Sneakers.Features.Command.Product
{
    public record DeleteProduct (Guid ProductId) : IRequest<EnumProduct>
    {
        
    }
}
