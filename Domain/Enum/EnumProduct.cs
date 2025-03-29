using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
{
    public enum EnumProduct
    {
        UploadProductSuccessfully,
        UploadProductFail,
        UpdateProductSuccessfully,
        UpdateProductFail,
        DeleteProductSuccessfully,
        DeleteProductFail,
        ProductExist
    }

    public static class EnumProductMessage
    {
        public static string GetMessage(this EnumProduct status)
        {

            return status switch
            {
                EnumProduct.UploadProductSuccessfully => "Product uploaded successfully.",
                EnumProduct.UploadProductFail => "Failed to upload product.",
                EnumProduct.UpdateProductSuccessfully => "Product updated successfully.",
                EnumProduct.UpdateProductFail => "Failed to update product.",
                EnumProduct.DeleteProductSuccessfully => "Product deleted successfully.",
                EnumProduct.DeleteProductFail => "Failed to delete product.",
                EnumProduct.ProductExist => "This product and this color exist, you should edit instead of adding.",
                _ => "An unknown error occurred."
            };
        }
    }
}
