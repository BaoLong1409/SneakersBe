using Domain.Entities;
using Domain.Enum;
using Domain.ViewModel.Order;
using Microsoft.AspNetCore.Mvc;
using Sneakers.Services.OrderService;
using Sneakers.Services.VnpayService;
using System.Collections.Specialized;
using System.Web;

namespace Sneakers.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;
        private readonly VnpayService _vnpayService;
        public OrderController(OrderService orderService, VnpayService vnpayService)
        {
            _orderService = orderService;
            _vnpayService = vnpayService;
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

        [HttpGet]
        [Route("order/pay/vnpay")]
        public async Task<IActionResult> CreateLinkToPay(Guid orderId)
        {
            var order = await _orderService.GetOrderById(orderId);
            if (order == null) {
                return BadRequest(new { status = EnumOrder.OrderNotFound.ToString(), message = "Order Not Found" });
            }
            var vnpayLink = _vnpayService.CreateRequestLink(order);
            return Ok(new
            {
                status = "Success",
                paymentUrl = vnpayLink
            });
        }

        //[HttpGet]
        //[Route("order/vnpay-ipn")]
        //public IActionResult GetResponseFromVnPay()
        //{

        //}

        [HttpGet]
        [Route("order/vnpay-return")]
        public async Task<IActionResult> VnpayReturn(String queryString)
        {
            try
            {
                (EnumTransactionStatus status, Guid orderId) = _vnpayService.CheckCodeUrl(queryString);
                if (status == EnumTransactionStatus.Success) {
                    EnumOrder orderStatus = await _orderService.UpdateOrderSuccessPayment(orderId);
                    if (orderStatus == EnumOrder.UpdateOrderSuccess)
                    {
                        return Ok(new
                        {
                            status = EnumTransactionStatus.Success,
                            message = EnumTransactionStatusExtensions.GetMessage(status)
                        });
                    }
                }

                return BadRequest(new
                {
                    status = status,
                    message = EnumTransactionStatusExtensions.GetMessage(status)
                });
            }
            catch (Exception ex) {
                return BadRequest(new
                {
                    status = EnumTransactionStatus.OtherError,
                    message = $"Exception: {ex.Message}"
                });
            }
        }
    }
}
