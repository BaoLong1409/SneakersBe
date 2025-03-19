using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
{
    public enum EnumUser
    {
        UpdateSuccessfully,
        UpdateFail,
        DeleteSuccessfully,
        DeleteFail,
        NotExist
    }

    public static class EnumUserMessage
    {
        public static string GetMessage(this EnumUser status)
        {
            return status switch
            {
                EnumUser.UpdateSuccessfully => "User information updated successfully.",
                EnumUser.UpdateFail => "Failed to update user information. Please check the details and try again.",
                EnumUser.DeleteSuccessfully => "User information deleted successfully.",
                EnumUser.DeleteFail => "Failed to delete user information. Please try again later.",
                EnumUser.NotExist => "User Information do not exist.",
                _ => "An unknown error occurred."
            };
        }
    }
}
