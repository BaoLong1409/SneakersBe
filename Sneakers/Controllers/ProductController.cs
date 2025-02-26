using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sneakers.Features.Queries.FeatureProducts;
using Sneakers.Features.Queries.Products;

namespace Sneakers.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        public ProductController(IConfiguration configuration, IMapper mapper, IUnitOfWork unitOfWork, IMediator mediator)
        {
            _configuration = configuration;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
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
        public async Task<IActionResult> GetProductById (Guid productId)
        {
            return Ok(await _mediator.Send(new GetProductById(productId)));
        }
    }
}
