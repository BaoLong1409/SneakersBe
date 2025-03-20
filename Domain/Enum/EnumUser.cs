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
        NotExist,
        InvalidEmail,
        NotConfirmed,
        InvalidPassword,
        LoginSuccess,
        LoginFail,
        CreateRoleFail,
        CreateUserFail
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
                EnumUser.InvalidEmail => "Your email is invalid.",
                EnumUser.NotConfirmed => "Your email has not been confirmed.",
                EnumUser.InvalidPassword => "Your Password is invalid.",
                EnumUser.LoginSuccess => "Login Successfully!",
                EnumUser.LoginFail => "Login fail!",
                EnumUser.CreateRoleFail => "Create new role failed!",
                EnumUser.CreateUserFail => "Create new user failed!",
                _ => "An unknown error occurred."
            };
        }
    }
}
