using Domain.Entities;
using Domain.Enum;
using Domain.ViewModel.Order;
using Microsoft.AspNetCore.Authorization;
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
        private readonly OrderDetailService _orderDetailService;
        public OrderController(OrderService orderService, VnpayService vnpayService, OrderDetailService orderDetailService)
        {
            _orderService = orderService;
            _vnpayService = vnpayService;
            _orderDetailService = orderDetailService;
        }
        [HttpPost]
        [Route("order/add")]

        public async Task <IActionResult> CreateOrder([FromBody] CreateOrderRequest orderRequest)
        {
            var (status, orderId) = await _orderService.AddOrder(orderRequest);
            
            return status switch
            {
                EnumOrder.CreateOrderSuccess => Ok(new { status = EnumOrder.CreateOrderSuccess.ToString(), message = "Create Order Success", orderId = orderId }),
                EnumOrder.CreateOrderFail => BadRequest(new { status = EnumOrder.CreateOrderFail.ToString(), message = "Create Order Fail" }),
                EnumOrder.NeedAddress => BadRequest(new { status = EnumOrder.NeedAddress.ToString(), message = "Need Address" }),
                _ => StatusCode(500, new { message = "Unknown Error" })
            };
        }

        [HttpGet]
        [Route("order/getAll")]
        public async Task<IActionResult> GetAllOrdersContr(Guid userId)
        {
            return Ok(await _orderService.GetAllOrdersSv(userId));
        }

        [HttpGet]
        [Route("order/admin/getPage")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPageOrders([FromQuery] int offset, [FromQuery] int limit, [FromQuery] string status)
        {
            return Ok(await _orderService.GetOrdersByLimit(offset, limit, status));
        }

        [HttpGet]
        [Route("order/getOrderInfo")]
        public async Task<IActionResult> GetOrderInfo(Guid orderId)
        {
            return Ok(await _orderService.GetOrderInfo(orderId));
        }

        [HttpGet]
        [Route("order/getOrderDetails")]
        public IActionResult GetOrderDetail(Guid orderId)
        {
            var orderDetails = _orderDetailService.GetAllOrderDetails(orderId).Result;
            if (!orderDetails.Any())
            {
                return BadRequest(new {message = "Products Doesn't Exist"});
            }
            return Ok(orderDetails);
        }

        [HttpPut]
        [Route("order/update")]
        public async Task <IActionResult> UpdateOrder([FromBody] OrderUpdateRequest order)
        {
            var orderStatus = await _orderService.UpdateOrder(order);
            return orderStatus switch
            {
                EnumOrder.UpdateOrderSuccess => Ok(new { status = EnumOrder.CreateOrderSuccess.ToString(), message = "Update Order Success" }),
                EnumOrder.UpdateQuantityFail => BadRequest(new { status = EnumOrder.UpdateQuantityFail.ToString(), message = "Update Product Quantity Fail" }),
                EnumOrder.OrderNotFound => BadRequest(new { status = EnumOrder.OrderNotFound.ToString(), message = "Order Not Found" }),
                _ => StatusCode(500, new { message = "Unknown Error" })
            };
        }

        [HttpPut]
        [Route("order/updateStatus")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusRequest request)
        {
            var orderStatus = await _orderService.UpdateOrderStatus(request);
            return orderStatus switch
            {
                EnumOrder.UpdateOrderSuccess => Ok(new { status = EnumOrder.CreateOrderSuccess.ToString(), message = "Update Order Success" }),
                EnumOrder.OrderNotFound => BadRequest(new { status = EnumOrder.OrderNotFound.ToString(), message = "Order Not Found" }),
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
            return Ok(new { url = vnpayLink });
        }

        //[HttpGet]
        //[Route("order/vnpay-ipn")]
        //public IActionResult GetResponseFromVnPay()
        //{

        //}

        [HttpGet]
        [Route("order/vnpay-return")]
        public async Task <IActionResult> VnpayReturn()
        {
            try
            {
                var queryString = HttpContext.Request.QueryString.Value;
                (EnumTransactionStatus status, Guid orderId) = _vnpayService.CheckCodeUrl(queryString);
                if (status == EnumTransactionStatus.Success) {
                    EnumOrder orderStatus = await _orderService.UpdateOrderSuccessPayment(orderId);
                    if (orderStatus == EnumOrder.UpdateQuantityFail)
                    {
                        return BadRequest(new { status = EnumOrder.UpdateQuantityFail.ToString(), message = "Update Product Quantity Fail" });
                    }

                    if (orderStatus == EnumOrder.AddOrderStatusSuccess)
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
