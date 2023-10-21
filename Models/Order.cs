using System;
using System.Collections.Generic;

namespace ShopQuanAo.Models
{
    public partial class Order
    {
        public Order()
        {
            Orderdetails = new HashSet<Orderdetail>();
        }

        public int Id { get; set; }
        public int? Idaccount { get; set; }
        public DateTime? Createdate { get; set; }
        public string? Email { get; set; }
        public string? Fullname { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public decimal? Total { get; set; }

        public virtual Account? IdaccountNavigation { get; set; }
        public virtual ICollection<Orderdetail> Orderdetails { get; set; }
    }
}
