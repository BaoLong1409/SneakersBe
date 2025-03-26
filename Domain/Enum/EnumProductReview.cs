using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
{
    public enum EnumProductReview
    {
        ReviewProductSuccessfully,
        ReviewProductFailed,
        UpdateReviewSuccessfully,
        UpdateProductFailed,
        DeleteProductReviewSuccessfully,
        DeleteProductReviewFailed,
        OrderDetailNotExist,
        OrderNotExist,
    }

    public static class EnumProductReviewMessage
    {
        public static string GetMessage(this EnumProductReview status)
        {
            return status switch
            {
                EnumProductReview.ReviewProductSuccessfully => "Review product successfully!",
                EnumProductReview.ReviewProductFailed => "Review product fail!",
                EnumProductReview.UpdateReviewSuccessfully => "Update product review successfully!",
                EnumProductReview.UpdateProductFailed => "Update product review failed!",
                EnumProductReview.DeleteProductReviewSuccessfully => "Delete product review successfully!",
                EnumProductReview.DeleteProductReviewFailed => "Delete product review failed!",
                EnumProductReview.OrderDetailNotExist => "Order detail does not exist!",
                EnumProductReview.OrderNotExist => "Order does not exist!",
                _ => "An unknown error occur!"
            };
        }
    }
}
