﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
{
    public enum EnumOrder
    {
        CreateOrderSuccess,
        UpdateOrderSuccess,
        AddOrderStatusSuccess,
        OrderNotFound,
        CreateOrderFail,
        NeedAddress
    }
}
