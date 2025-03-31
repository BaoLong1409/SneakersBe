using AutoMapper;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sneakers.Features.Queries.FeatureProducts;
using Sneakers.Services.SizeService;

namespace Sneakers.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class SizeController : Controller
    {
        private readonly SizeService _sizeService;

        public SizeController(SizeService sizeService)
        {
            _sizeService = sizeService;
        }

        [HttpGet]
        [Route("size/getAllSizes")]
        public IActionResult GetAllSizes()
        {
            var sizes = _sizeService.GetAllSizes();
            return Ok(sizes);
        }

        [HttpGet]
        [Route("size/getAvailableSizes")]
        public async Task<IActionResult> GetAvailableSizes(Guid productId, string colorName)
        {
            var sizes = await _sizeService.GetAvailableSizes(productId, colorName);
            return Ok(sizes);
        }
    }
}
