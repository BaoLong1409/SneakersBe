using Domain.Entities;
using Domain.Enum;
using Domain.ViewModel.Order;
using Microsoft.AspNetCore.Mvc;
using Sneakers.Services.OrderService;

namespace Sneakers.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;
        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost]
        [Route("order/addOrder")]

        public IActionResult CreateOrder([FromBody] CreateOrderRequest orderRequest)
        {
            var (status, orderId) = _orderService.AddOrder(orderRequest);
            return status switch
            {
                EnumOrder.CreateOrderSuccess => Ok(new { status = EnumOrder.CreateOrderSuccess.ToString(), message = "Create Order Success", orderId = orderId }),
                EnumOrder.CreateOrderFail => BadRequest(new { status = EnumOrder.CreateOrderFail.ToString(), message = "Create Order Fail" }),
                EnumOrder.NeedAddress => BadRequest(new { status = EnumOrder.NeedAddress.ToString(), message = "Need Address" }),
                _ => StatusCode(500, new { message = "Unknown Error" })
            };
        }
    }
}
