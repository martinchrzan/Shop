using System;
using System.Collections.Generic;
using System.Text;

namespace ShopModels
{
    public class BasketItem
    {
        public string UserToken { get; set; }

        public ProductCount ProductCount { get; set; }
    }
}
