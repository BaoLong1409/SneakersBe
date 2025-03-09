using Domain.Entities;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Sneakers.Services.VnpayService
{
    public class VnpayService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public VnpayService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public string CreateRequestLink(Order orderInfo)
        {

            string vnp_ReturnUrl = _configuration["VnPay:ReturnUrl"];
            string vnp_Url = _configuration["VnPay:Url"];
            string vnp_TmnCode = _configuration["VnPay:TmnCode"];
            string vnp_HashSecret = _configuration["VnPay:HashSecret"];

            var vnp_Params = new Dictionary<string, string>();
            vnp_Params.Add("vnp_Version", "2.1.0");
            vnp_Params.Add("vnp_Command", "pay");
            vnp_Params.Add("vnp_TmnCode", vnp_TmnCode);
            //string locale = form.Get("language");//en= English, vn=Tiếng Việt
            //if (!string.IsNullOrEmpty(locale))
            //{
            //    vnp_Params.Add("vnp_Locale", locale);
            //}
            //else
            //{
            //}
            vnp_Params.Add("vnp_Locale", "vn");
            vnp_Params.Add("vnp_OrderInfo", $"Thanh toan hoa don {orderInfo.Id}. So tien: {(int)orderInfo.TotalMoney * 10000} VND");
            vnp_Params.Add("vnp_OrderType", "other");
            vnp_Params.Add("vnp_ExpireDate", DateTime.UtcNow.AddHours(7).AddMinutes(15).ToString("yyyyMMddHHmmss"));

            vnp_Params.Add("vnp_CurrCode", "VND");
            string txnRef = orderInfo.Id.ToString("N");
            vnp_Params.Add("vnp_TxnRef", txnRef);
            vnp_Params.Add("vnp_Amount", ((int)orderInfo.TotalMoney * 10000).ToString());
            vnp_Params.Add("vnp_ReturnUrl", vnp_ReturnUrl);
            vnp_Params.Add("vnp_IpAddr", GetIpAddress());
            vnp_Params.Add("vnp_CreateDate", DateTime.UtcNow.ToString("yyyyMMddHHmmss"));

            vnp_Params = vnp_Params.OrderBy(o => o.Key).ToDictionary(k => k.Key, v => v.Value);

            StringBuilder data = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in vnp_Params)
            {
                if (!String.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }
            string queryString = data.ToString();

            vnp_Url += "?" + queryString;
            String signData = queryString;
            if (signData.Length > 0)
            {

                signData = signData.Remove(data.Length - 1, 1);
            }
            string vnp_SecureHash = HmacSHA512(vnp_HashSecret, signData);
            vnp_Url += "vnp_SecureHash=" + vnp_SecureHash;

            return vnp_Url;

        }

        private string GetIpAddress()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null) return "127.0.0.1"; // Fallback nếu HttpContext null

                // Lấy IP từ header nếu request qua Proxy/Load Balancer
                var ipAddress = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                if (string.IsNullOrEmpty(ipAddress))
                {
                    ipAddress = httpContext.Connection.RemoteIpAddress?.ToString();
                }

                // Chuyển IPv6 loopback (::1) thành IPv4 127.0.0.1
                if (ipAddress == "::1")
                {
                    ipAddress = "127.0.0.1";
                }

                return ipAddress ?? "127.0.0.1"; // Nếu không lấy được thì fallback về localhost
            }
            catch (Exception ex)
            {
                return "Invalid IP: " + ex.Message;
            }
        }


        private string HmacSHA512(string key, string inputData)
        {
            var hash = new StringBuilder();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }

            return hash.ToString();
        }
    }
}
