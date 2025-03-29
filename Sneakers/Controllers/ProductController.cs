using AutoMapper;
using Domain.Enum;
using Domain.Interfaces;
using Domain.ViewModel.Product;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sneakers.Features.Queries.FeatureProducts;
using Sneakers.Features.Queries.Products;
using Sneakers.Services.ColorService;
using Sneakers.Services.ProductService;

namespace Sneakers.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly ColorService _colorService;
        private readonly ProductService _productService;
        public ProductController(IMapper mapper, IUnitOfWork unitOfWork, IMediator mediator, ColorService colorService, ProductService productService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _colorService = colorService;
            _productService = productService;
        }

        [HttpPost]
        [Route("product/uploadNewProduct")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadNewProduct([FromBody] UploadNewProductRequest request)
        {
            var status = await _productService.UploadNewProduct(request);
            return status switch
            {
                EnumProduct.UploadProductSuccessfully => Ok(new {message = status.GetMessage()}),
                EnumProduct.UploadProductFail => BadRequest(new { message = status.GetMessage()}),
                EnumProduct.ProductExist => BadRequest(new { message = status.GetMessage()}),
                _ => StatusCode(500, status.GetMessage())
            };
        }

        [HttpGet]
        [Route("product/getAllFeatureProducts")]
        public async Task<IActionResult> GetFeatureProducts()
        {
            return Ok(await _mediator.Send(new GetAllFeatureProducts()));
        }

        [HttpGet]
        [Route("product/getAll")]
        public async Task<IActionResult> GetAllProducts()
        {
            return Ok(await _mediator.Send(new GetAllProducts()));
        }

        [HttpGet]
        [Route("product/searchAllProducts")]
        public async Task<IActionResult> SeachAllProducts(string? keyword)
        {
            return Ok(await _mediator.Send(new SearchAllProducts(keyword ?? "")));
        }

        [HttpGet]
        [Route("product/getAllWCondition")]
        public async Task<IActionResult> GetAllProductsWithCondition([FromQuery] int[] priceFilter)
        {
            return Ok(await _productService.GetProductsWithCondition(priceFilter));
        }

        [HttpGet]
        [Route("product/getAllByCategory")]
        public async Task<IActionResult> GetAllProductsByCategory([FromQuery] GetProductsByCategoryReq req)
        {
            if (req.CategoryName == "null") req.CategoryName = null;
            if (req.BrandName == "null") req.BrandName = null;
            return Ok(await _productService.GetAllProductsByCategory(req));
        }

        [HttpGet]
        [Route("product/getRecommendProduct")]
        public async Task<IActionResult> GetRecommendProducts(String userId)
        {
            return Ok(await _mediator.Send(new GetRecommendProducts(userId)));
        }

        [HttpGet]
        [Route("product/getProductById")]
        public async Task<IActionResult> GetProductById (Guid productId, String color)
        {
            return Ok(await _mediator.Send(new GetProductById(productId, color)));
        }

        [HttpGet]
        [Route("product/getImageColorsProduct")]
        public async Task<IActionResult> GetImageColorsProduct(Guid productId)
        {
            var colors = await _colorService.GetAllProductColors(productId);
            var productThumbnailColors = await _productService.GetImageProductColor(productId, colors);
            return Ok(productThumbnailColors);
        }

        [HttpGet]
        [Route("product/getAvailableSizes")]
        public async Task<IActionResult> GetAvaiableSizes(Guid productId, String colorName)
        {
            var color = await _colorService.GetColorFromColorName(colorName);

            if (color == null) {
                return NotFound("Color Not Found");
            }
            
            var availableProducts = await _productService.GetAvailableProducts(productId, color.Id);
            return Ok(availableProducts);
        }
    }
}
