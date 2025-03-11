using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.SeedData
{
    public static class SeedData
    {
        public static readonly Guid CODPaymentId = Guid.Parse("b3c1f7a4-92d5-4b19-a7e5-6d8b2a9f3d44");
        public static readonly Guid VNPayPaymentId = Guid.Parse("f47e3b92-5c1d-4e06-9ea2-8b1d77f8c123");

        public static readonly Guid StandardShippingId = Guid.Parse("7e2f4c38-8a5b-402a-b8d1-5e9fbc3d7a92");
        public static readonly Guid ExpressShippingId = Guid.Parse("d8a9c347-6e5a-4b11-bf9d-2f4e9c1a7d55");
        public static readonly Guid UltraFastShippingId = Guid.Parse("c3b82e7d-1f92-4a50-a6b3-7d9e4f5c2a88");

    }
}
