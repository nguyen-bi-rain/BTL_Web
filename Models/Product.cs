using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace ShopQuanAo.Models
{
    public partial class Product
    {
        public Product()
        {
            Orderdetails = new HashSet<Orderdetail>();
        }

        public int Id { get; set; }
        [Display(Name = "Tên sản phẩm")]
        [StringLength(50,ErrorMessage = "Tên không được quá 50 kí tự")]
        [Required(ErrorMessage ="Hãy nhập đủ tên")]
        public string? Name { get; set; }
        [Display(Name = "Giá sản phẩm")]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Hãy nhập giá vào")]
        public decimal? Price { get; set; }
        [Display(Name = "số lượng sản phẩm")]
        [Required(ErrorMessage = "Hãy nhập số lượng")]
        public int? Quantity { get; set; }
        [Display(Name = "giá đã giảm")]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Hãy nhập giá vào")]
        public decimal? Promationprice { get; set; }
        [Display(Name = "Mô tả sản phẩm")]
        [Required(ErrorMessage = "Hãy nhập mô tả")]
        public string? Description { get; set; }
        [Display(Name = "Ảnh Sản Phẩm")]
        public string? Image { get; set; }
        [Display(Name = "Trạng thái sản phẩm")]
        [Required(ErrorMessage = "Hãy chọn trạng thái sản phẩm là mới hay cũ")]
        public bool? Newproduct { get; set; }
        [Display(Name = "Nhãn hàng")]
        public int? Idcategory { get; set; }

        public virtual Category? IdcategoryNavigation { get; set; }
        public virtual ICollection<Orderdetail> Orderdetails { get; set; }
        [NotMapped]
        public IFormFile? image { get; set; }
    }
}
