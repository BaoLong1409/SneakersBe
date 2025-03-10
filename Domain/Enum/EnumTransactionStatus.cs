using System;

namespace Domain.Enum
{
    public enum EnumTransactionStatus
    {
        Success = 00, // "Transaction successful"
        Suspicious = 07, // "Deducted successfully, but transaction is suspected (fraud or unusual activity)"
        UnregisteredInternetBanking = 09, // "Transaction failed: Card/Account not registered for Internet Banking"
        IncorrectAuthentication = 10, // "Transaction failed: Incorrect card/account authentication more than 3 times"
        PaymentTimeout = 11, // "Transaction failed: Payment timeout. Please try again."
        AccountLocked = 12, // "Transaction failed: Card/Account is locked"
        IncorrectOTP = 13, // "Transaction failed: Incorrect OTP. Please try again."
        CustomerCancelled = 24, // "Transaction failed: Customer canceled the transaction"
        InsufficientBalance = 51, // "Transaction failed: Insufficient account balance"
        ExceededTransactionLimit = 65, // "Transaction failed: Exceeded daily transaction limit"
        BankMaintenance = 75, // "Payment bank under maintenance"
        ExceededPasswordAttempts = 79, // "Transaction failed: Exceeded allowed payment password attempts. Please try again."
        OtherError = 99 // "Other errors (not listed)"
    }

    public static class EnumTransactionStatusExtensions
    {
        public static string GetMessage(this EnumTransactionStatus status)
        {
            return status switch
            {
                EnumTransactionStatus.Success => "Transaction successful",
                EnumTransactionStatus.Suspicious => "Deducted successfully, but transaction is suspected (fraud or unusual activity)",
                EnumTransactionStatus.UnregisteredInternetBanking => "Transaction failed: Card/Account not registered for Internet Banking",
                EnumTransactionStatus.IncorrectAuthentication => "Transaction failed: Incorrect card/account authentication more than 3 times",
                EnumTransactionStatus.PaymentTimeout => "Transaction failed: Payment timeout. Please try again.",
                EnumTransactionStatus.AccountLocked => "Transaction failed: Card/Account is locked",
                EnumTransactionStatus.IncorrectOTP => "Transaction failed: Incorrect OTP. Please try again.",
                EnumTransactionStatus.CustomerCancelled => "Transaction failed: Customer canceled the transaction",
                EnumTransactionStatus.InsufficientBalance => "Transaction failed: Insufficient account balance",
                EnumTransactionStatus.ExceededTransactionLimit => "Transaction failed: Exceeded daily transaction limit",
                EnumTransactionStatus.BankMaintenance => "Payment bank under maintenance",
                EnumTransactionStatus.ExceededPasswordAttempts => "Transaction failed: Exceeded allowed payment password attempts. Please try again.",
                EnumTransactionStatus.OtherError => "Other errors (not listed)",
                _ => "Unknown error"
            };
        }

        public static EnumTransactionStatus GetTransactionStatus(string responseCode)
        {
            return responseCode switch
            {
                "00" => EnumTransactionStatus.Success,
                "07" => EnumTransactionStatus.Suspicious,
                "09" => EnumTransactionStatus.UnregisteredInternetBanking,
                "10" => EnumTransactionStatus.IncorrectAuthentication,
                "11" => EnumTransactionStatus.PaymentTimeout,
                "12" => EnumTransactionStatus.AccountLocked,
                "13" => EnumTransactionStatus.IncorrectOTP,
                "24" => EnumTransactionStatus.CustomerCancelled,
                "51" => EnumTransactionStatus.InsufficientBalance,
                "65" => EnumTransactionStatus.ExceededTransactionLimit,
                "75" => EnumTransactionStatus.BankMaintenance,
                "79" => EnumTransactionStatus.ExceededPasswordAttempts,
                _ => EnumTransactionStatus.OtherError
            };
        }
    }
}
