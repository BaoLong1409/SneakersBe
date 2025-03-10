using Domain.Entities;
using Domain.Enum;
using System.Collections.Specialized;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

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
            vnp_Params.Add("vnp_OrderInfo", $"Thanh toan hoa don {orderInfo.Id}. So tien: {(int)orderInfo.TotalMoney * 25000 * 100} VND");
            vnp_Params.Add("vnp_OrderType", "other");
            vnp_Params.Add("vnp_ExpireDate", DateTime.UtcNow.AddHours(7).AddMinutes(15).ToString("yyyyMMddHHmmss"));

            vnp_Params.Add("vnp_CurrCode", "VND");
            string txnRef = orderInfo.Id.ToString("N");
            vnp_Params.Add("vnp_TxnRef", txnRef);
            vnp_Params.Add("vnp_Amount", ((int)orderInfo.TotalMoney * 25000 * 100).ToString());
            vnp_Params.Add("vnp_ReturnUrl", vnp_ReturnUrl);
            vnp_Params.Add("vnp_IpAddr", GetIpAddress());
            vnp_Params.Add("vnp_CreateDate", DateTime.UtcNow.ToString("yyyyMMddHHmmss"));

            vnp_Params = vnp_Params.OrderBy(o => o.Key).ToDictionary(k => k.Key, v => v.Value);

            string queryString = GetResponseData(vnp_Params);

            vnp_Url += "?" + queryString;
            String signData = queryString;
            if (signData.Length > 0)
            {
                signData = signData.Remove(signData.Length - 1, 1);
            }
            string vnp_SecureHash = HmacSHA512(vnp_HashSecret, signData);
            vnp_Url += "vnp_SecureHash=" + vnp_SecureHash;

            return vnp_Url;
        }

        public (EnumTransactionStatus status, Guid orderId) CheckCodeUrl(String queryString)
        {
            string vnp_ReturnUrl = _configuration["VnPay:ReturnUrl"];
            string vnp_Url = _configuration["VnPay:Url"];
            string vnp_TmnCode = _configuration["VnPay:TmnCode"];
            string vnp_HashSecret = _configuration["VnPay:HashSecret"];
            Dictionary<string, string> vnp_Params = new Dictionary<string, string>();

            NameValueCollection queryCollection = HttpUtility.ParseQueryString(queryString);
            foreach (string key in queryCollection)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    string value = queryCollection[key] ?? string.Empty;
                    vnp_Params.Add(key, value);
                }
            }

            vnp_Params["vnp_Amount"] = (Int64.Parse(vnp_Params["vnp_Amount"]) / 100).ToString();

            string vnp_ResponseCode = vnp_Params["vnp_ResponseCode"];
            string orderIdQuery = vnp_Params["vnp_TxnRef"];
            Guid orderId = Guid.ParseExact(orderIdQuery, "N");

            if (vnp_Params.TryGetValue("vnp_SecureHash", out var secureHash))
            {
            }
            else
            {
                secureHash = string.Empty;
            }

            if (!ValidateSignature(secureHash, vnp_HashSecret, vnp_Params))
            {
                throw new Exception("Invalid signature");
            }
            EnumTransactionStatus status = EnumTransactionStatusExtensions.GetTransactionStatus(vnp_ResponseCode);
            return (status, orderId);
        }

        public bool ValidateSignature(string inputHash, string secretKey, Dictionary<string, string> vnp_Params)
        {
            string rspRaw = GetResponseData(vnp_Params);
            string myCheckSum = HmacSHA512(secretKey, rspRaw);
            return myCheckSum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }

        private string GetResponseData(Dictionary<string, string> vnp_params)
        {
            StringBuilder sb = new StringBuilder();
            if (vnp_params.ContainsKey("vnp_SecureHashType"))
            {
                vnp_params.Remove("vnp_SecureHashType");
            }
            if (vnp_params.ContainsKey("vnp_SecureHash"))
            {
                vnp_params.Remove("vnp_SecureHash");
            }

            foreach (var item in vnp_params)
            {
                if (!String.IsNullOrEmpty(item.Value))
                {
                    sb.Append(WebUtility.UrlEncode(item.Key) + "=" + WebUtility.UrlEncode(item.Value) + "&");
                }
            }

            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
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
