using Microsoft.AspNetCore.Mvc;
using Sneakers.Services.ColorService;
using Sneakers.Services.SizeService;

namespace Sneakers.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ColorController : Controller
    {
        private readonly ColorService _colorService;

        public ColorController(ColorService colorService)
        {
            _colorService = colorService;
        }

        [HttpGet]
        [Route("color/getAll")]
        public IActionResult GetAllColors()
        {
            return Ok(_colorService.GetAllColors());
        }

        [HttpGet]
        [Route("color/getProductColors")]
        public async Task<IActionResult> GetProductColors(Guid productId)
        {
            var colors = await _colorService.GetAllProductColors(productId);
            return Ok(colors);
        }
    }
}
