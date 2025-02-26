using AutoMapper;
using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces;
using Domain.ViewModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sneakers.Services.CartService;

namespace Sneakers.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class CartController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly CartService _cartService;
        public CartController(IConfiguration configuration, IMapper mapper, IUnitOfWork unitOfWork, IMediator mediator, CartService cartService)
        {
            _configuration = configuration;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _cartService = cartService;
        }

        [HttpPost]
        [Route("cart/addProduct")]
        public async Task<IActionResult> AddProductToCart([FromBody] ManageProductInCartDto product, Guid userId)
        {
            var cart = await _unitOfWork.Cart.GetFirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null) {
                return NotFound("Cart Not Found");
            }

            var productStockQuantity = await _unitOfWork.ProductQuantity
                .GetFirstOrDefaultAsync(c => 
                c.ProductId == product.ProductId && 
                c.SizeId == product.SizeId && 
                c.ColorId == product.ColorId
                );
            if (productStockQuantity == null) {
                return NotFound("Product Not Found");
            }

            if (productStockQuantity.StockQuantity < product.Quantity)
            {
                return BadRequest(new { message = "Not Enough Product In Stock" });
            }

            var productToAddCart = _mapper.Map<ProductCart>(product);
            productToAddCart.CartId = cart.Id;

            _unitOfWork.ProductCart.Add(productToAddCart);
            _unitOfWork.Complete();

            return Ok(new { message = "Add Product Complete" });
        }

        [HttpGet]
        [Route("cart/getProduct")]
        public async Task<IActionResult> GetProductFromCart(Guid userId)
        {
            var products = await _cartService.GetProductInCartsAsync(userId);
            return Ok(products);
        }

        [HttpPut]
        [Route("cart/updateProduct")]
        public async Task<IActionResult> UpdateProductInCart([FromBody] ManageProductInCartDto product, Guid userId)
        {
            var status = await _cartService.UpdateProductInCart(product, userId);
            return status switch
            {
                EnumProductCart.CartNotFound => NotFound("Cart Not Found"),
                EnumProductCart.ProductNotFound => NotFound("Product Not Found"),
                EnumProductCart.NotEnoughInStock => BadRequest(new { message = "Not Enough Product In Stock" }),
                EnumProductCart.NotExist => Ok(new { message = "Product does not exist, but add completely" }),
                EnumProductCart.Success => Ok(new { message = "Update Product Completely" }),
                _ => StatusCode(500, "Unknown Error")
            };
        }

        [HttpDelete]
        [Route("cart/deleteProduct")]
        public async Task<IActionResult> DeleteProductInCart([FromBody] ManageProductInCartDto product, Guid userId)
        {
            var status = await _cartService.DeleteProductInCart(product, userId);
            return status switch
            {
                EnumProductCart.ProductNotFound => NotFound("Product Not Found"),
                EnumProductCart.CartNotFound => NotFound("Cart Not Found"),
                EnumProductCart.Success => Ok(new { message = "Delete Product Completely" }),
                _ => StatusCode(500, "Unknown Error")
            };
        }
    }
}
