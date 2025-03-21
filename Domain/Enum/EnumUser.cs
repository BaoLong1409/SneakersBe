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
        InvalidPhoneNumber,
        InvalidEmail,
        NotConfirmed,
        InvalidPassword,
        LoginSuccess,
        LoginFail,
        CreateRoleFail,
        CreateUserFail,
        TokenInvalid,
        ChangePasswordSuccess,
        ChangePasswordFail,
        DuplicatePassword
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
                EnumUser.TokenInvalid => "Your token is invalid!",
                EnumUser.ChangePasswordSuccess => "Your password has been changed successfully!",
                EnumUser.ChangePasswordFail => "Your password change failed!",
                EnumUser.DuplicatePassword => "Your old and new password have to different!",
                EnumUser.InvalidPhoneNumber => "Your phone number is invalid!",
                _ => "An unknown error occurred."
            };
        }
    }
}
