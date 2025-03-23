using Domain.Enum;
using Microsoft.Extensions.Caching.Memory;
using Sneakers.Services.UserService;
using System.Security.Cryptography;

namespace Sneakers.Services.OTPService
{
    public class OTPService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _expiry = TimeSpan.FromMinutes(3);
        private const int MaxRequestPerHour = 7;
        private const int ResetOtpTime = 30;
        public OTPService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public (EnumUser, string?) GenerateAndStoreOTP(string userEmail)
        {
            string otp = GenerateSecureOTP();
            _memoryCache.Set(userEmail, otp.ToString(), _expiry);
            var cacheKey = $"OTP_Request_{userEmail}";

            if (!_memoryCache.TryGetValue(cacheKey, out int reqCount)) {
                reqCount = 0;
            }

            if (reqCount >= MaxRequestPerHour)
            {
                return (EnumUser.SpamOTP, null);
            }

            _memoryCache.Set(cacheKey, reqCount + 1, TimeSpan.FromMinutes(ResetOtpTime));
            return (EnumUser.SentOTP, otp);
        }

        public bool ValidateOTP(string userEmail, string inputOTP)
        {
            if (_memoryCache.TryGetValue(userEmail, out var storedOTP) && storedOTP.ToString() == inputOTP.ToString())
            {
                _memoryCache.Remove(userEmail);
                return true;
            }

            return false;
        }

        private string GenerateSecureOTP(int length = 6)
        {
            const string chars = "0123456789";
            byte[] data = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(data);
            }

            char[] otp = new char[length];
            for (int i = 0; i < length; i++)
            {
                otp[i] = chars[data[i] % chars.Length];
            }

            return new string(otp);
        }
    }
}
