using Domain.Enum;
using Domain.ViewModel.ProductReview;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sneakers.Services.ProductService;

namespace Sneakers.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class ProductReviewController : Controller
    {
        private readonly ProductReviewService _productReviewService;
        public ProductReviewController(ProductReviewService productReviewService)
        {
            _productReviewService = productReviewService;
        }

        [HttpPost]
        [Route("review/post")]
        [Authorize]
        public async Task<IActionResult> ReviewProduct([FromBody] ProductReviewRequest request)
        {
            var status = await _productReviewService.PostProductReview(request);
            return status switch
            {
                EnumProductReview.ReviewProductSuccessfully => Ok(new { message = status.GetMessage() }),
                EnumProductReview.ReviewProductFailed => BadRequest(new {message = status.GetMessage()}),
                _ => StatusCode(500, "Unknown Error")
            };
        }

        [HttpGet]
        [Route("review/getToReview")]
        [Authorize]
        public async Task<IActionResult> GetProductsWaittingReview(Guid userId)
        {
            return Ok(await _productReviewService.GetProductsWaittingReview(userId));
        }

        [HttpGet]
        [Route("review/getComment")]
        public async Task<IActionResult> GetProductComment([FromQuery] GetCommentsOfProductRequest req)
        {
            return Ok(await _productReviewService.GetCommentsOfProducts(req));
        }
    }
}
