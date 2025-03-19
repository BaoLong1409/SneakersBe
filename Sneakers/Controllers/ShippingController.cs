using Domain.Entities;
using Domain.Enum;
using Domain.ViewModel.User;
using Microsoft.AspNetCore.Mvc;
using Sneakers.Services.ShippingService;
using Sneakers.Services.UserService;

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

        [HttpPost]
        [Route("shipping/addUserAddress")]
        public async Task<IActionResult> AddUserAddress([FromBody] ShippingInfoDto shippingInfo)
        {
            var (newList, status) = await _shippingService.AddShippingInfo(shippingInfo);
            return status switch
            {
                EnumShippingInfo.AddSuccessfully => Ok(new { message = status.GetMessage(), data = newList }),
                _ => StatusCode(500, new { message = status.GetMessage() })
            };
        }

        [HttpGet]
        [Route("shipping/getUserAddress")]
        public async Task<IActionResult> GetListAddress(Guid userId)
        {
            return Ok(await _shippingService.GetShippingInfo(userId));
        }

        [HttpPut]
        [Route("shipping/updateUserAddress")]
        public async Task<IActionResult> UpdateUserAddress([FromBody] ShippingInfoDto shippingInfo)
        {
            var (newList, status) = await _shippingService.UpdateShippingInfo(shippingInfo);
            return status switch
            {
                EnumShippingInfo.UpdateSuccessfully => Ok(new { message = status.GetMessage(), data = newList }),
                EnumShippingInfo.NotExist => BadRequest(new { message = status.GetMessage()}),
                _ => StatusCode(500, new { message = status.GetMessage() })
            };
        }

        [HttpDelete]
        [Route("shipping/deleteUserAddress")]
        public async Task<IActionResult> DeleteUserAddress(Guid shippingInfoId)
        {
            var (newList, status) = await _shippingService.DeleteShippingInfo(shippingInfoId);
            return status switch
            {
                EnumShippingInfo.DeleteSuccessfully => Ok(new { message = status.GetMessage(), data = newList }),
                EnumShippingInfo.NotExist => BadRequest(new { message = status.GetMessage() }),
                _ => StatusCode(500, new { message = status.GetMessage() })
            };
        }


    }
}
