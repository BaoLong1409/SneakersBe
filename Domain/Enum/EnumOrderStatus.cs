using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
{
    public enum EnumOrderStatus
    {
        Pending,
        Failed,
        Refunded,
        Processing,
        Shipped,
        Delivered,
        Completed
    }
}
