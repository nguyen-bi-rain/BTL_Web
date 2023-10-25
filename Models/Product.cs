using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopQuanAo.Models
{
    public partial class Product
    {
        public Product()
        {
            Orderdetails = new HashSet<Orderdetail>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public decimal? Promationprice { get; set; }
        public string? Description { get; set; }
        [NotMapped]
        public FormFile? Image { get; set; }
        public bool? Newproduct { get; set; }
        public int? Idcategory { get; set; }

        public virtual Category? IdcategoryNavigation { get; set; }
        public virtual ICollection<Orderdetail> Orderdetails { get; set; }
    }
}
