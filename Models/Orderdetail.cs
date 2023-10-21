using System;
using System.Collections.Generic;

namespace ShopQuanAo.Models
{
    public partial class Orderdetail
    {
        public int Idorder { get; set; }
        public int Idproduct { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public decimal? Subtotal { get; set; }

        public virtual Order IdorderNavigation { get; set; } = null!;
        public virtual Product IdproductNavigation { get; set; } = null!;
    }
}
