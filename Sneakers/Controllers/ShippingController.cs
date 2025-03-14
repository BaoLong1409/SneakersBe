using Microsoft.AspNetCore.Mvc;
using Sneakers.Services.ShippingService;

namespace Sneakers.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class ShippingController : Controller
    {
        private readonly ShippingService _shippingService;
        public ShippingController(ShippingService shippingService)
        {
            _shippingService = shippingService;
        }
        [HttpGet]
        [Route("shipping/getAll")]

        public IActionResult GetAllShippingMethod()
        {
            var shippingMethods = _shippingService.GetAllShippingMethods();
            return Ok(shippingMethods);
        }
    }
}
