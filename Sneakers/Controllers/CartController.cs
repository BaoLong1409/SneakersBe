using AutoMapper;
using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces;
using Domain.ViewModel.Cart;
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
            var res = await _cartService.AddProductToCart(product, userId);
            return res switch
            {
                EnumProductCart.CartNotFound => NotFound(new { status = EnumProductCart.CartNotFound.ToString(), message = "Cart Not Found" }),
                EnumProductCart.ProductNotFound => NotFound(new { status = EnumProductCart.ProductNotFound.ToString(), message = "Product Not Found" }),
                EnumProductCart.NotEnoughInStock => BadRequest(new { status = EnumProductCart.NotEnoughInStock.ToString(), message = "Not Enough In Stock" }),
                EnumProductCart.Success => Ok(new {status = EnumProductCart.Success.ToString(), message = "Add Product To Cart Completely" }),
                _ => StatusCode(500, new { message = "Unknown Error" } )
            };
        }

        [HttpGet]
        [Route("cart/getProduct")]
        public async Task<IActionResult> GetProductsFromCart(Guid userId)
        {
            var products = await _cartService.GetProductsInCartAsync(userId);
            return Ok(products);
        }

        [HttpPut]
        [Route("cart/updateProduct")]
        public async Task<IActionResult> UpdateProductInCart([FromBody] ManageProductInCartDto product, Guid userId)
        {
            var status = await _cartService.UpdateProductInCart(product, userId);
            return status switch
            {
                EnumProductCart.CartNotFound => NotFound(new { status = EnumProductCart.CartNotFound.ToString(), message = "Cart Not Found" }),
                EnumProductCart.ProductNotFound => NotFound(new { status = EnumProductCart.ProductNotFound.ToString(), message = "Product Not Found" }),
                EnumProductCart.NotEnoughInStock => BadRequest(new { status = EnumProductCart.NotEnoughInStock.ToString(), message = "Not Enough In Stock" }),
                EnumProductCart.NotExist => Ok(new { status = EnumProductCart.NotExist.ToString(), message = "Product does not exist, but add completely" }),
                EnumProductCart.Success => Ok(new { status = EnumProductCart.Success.ToString(), message = "Update Product Completely" }),
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
                EnumProductCart.CartNotFound => NotFound(new { status = EnumProductCart.CartNotFound.ToString(), message = "Cart Not Found" }),
                EnumProductCart.ProductNotFound => NotFound(new { status = EnumProductCart.ProductNotFound.ToString(), message = "Product Not Found" }),
                EnumProductCart.Success => Ok(new { status = EnumProductCart.Success.ToString(), message = "Update Product Completely" }),
                _ => StatusCode(500, "Unknown Error")
            };
        }

        [HttpDelete]
        [Route("cart/deleteAllProducts")]
        public async Task<IActionResult> DeleteAllProductsInCart(Guid userId)
        {
            var status = await _cartService.DeleteAllProductsInCart(userId);
            return status switch
            {
                EnumProductCart.ProductNotFound => NotFound(new { status = EnumProductCart.ProductNotFound.ToString(), message = "Product Not Found" }),
                EnumProductCart.Success => Ok(new { status = EnumProductCart.Success.ToString(), message = "Update Product Completely" }),
                _ => StatusCode(500, "Unknown Error")
            };
        }
    }
}
