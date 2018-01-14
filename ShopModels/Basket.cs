using System.Collections.Generic;

namespace ShopModels
{
    public class Basket
    {
        public int Id { get; set; }

        public string UserToken { get; set; }

        public List<ProductCount> Products { get; set; }
    }
}