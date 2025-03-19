using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
{
    public enum EnumShippingInfo
    {
        AddSuccessfully,
        AddFail,
        UpdateSuccessfully,
        UpdateFail,
        DeleteSuccessfully,
        DeleteFail,
        NotExist
    }

    public static class EnumShippingMessage
    {
        public static string GetMessage(this EnumShippingInfo status)
        {
            return status switch
            {
                EnumShippingInfo.AddSuccessfully => "Shipping information added successfully.",
                EnumShippingInfo.AddFail => "Failed to add shipping information. Please try again.",
                EnumShippingInfo.UpdateSuccessfully => "Shipping information updated successfully.",
                EnumShippingInfo.UpdateFail => "Failed to update shipping information. Please check the details and try again.",
                EnumShippingInfo.DeleteSuccessfully => "Shipping information deleted successfully.",
                EnumShippingInfo.DeleteFail => "Failed to delete shipping information. Please try again later.",
                EnumShippingInfo.NotExist => "Shipping Information do not exist.",
                _ => "An unknown error occurred."
            };
        }
    }
}
