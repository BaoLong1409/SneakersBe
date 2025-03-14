using Microsoft.AspNetCore.Mvc;
using Sneakers.Services.PaymentService;

namespace Sneakers.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class PaymentController : Controller
    {
        private readonly PaymentService _paymentService;
        public PaymentController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        [Route("payment/getAll")]
        public IActionResult GetAllPayments()
        {
            var payments = _paymentService.GetAllPayments();
            return Ok(payments);
        }

    }
}
