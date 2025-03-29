using Domain.Enum;
using Domain.ViewModel.Product;
using Domain.ViewModel.ProductReview;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Sneakers.Features.Command.Product
{
    public record UploadNewProduct (UploadNewProductRequest Request) : IRequest<EnumProduct>
    {
    }
}
