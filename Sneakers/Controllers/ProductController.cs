using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
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
        [Route("product/getImageProductColors")]
        public async Task<IActionResult> GetImageProductColors(Guid productId)
        {
            var colors = await _colorService.GetAllProductColors(productId);
            var productThumbnailColors = await _productService.GetImageProductColor(productId, colors);
            return Ok(productThumbnailColors);
        }
    }
}
