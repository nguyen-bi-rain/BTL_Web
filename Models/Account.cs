using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopQuanAo.Models
{
    public partial class Account
    {
        public Account()
        {
            Orders = new HashSet<Order>();
        }
        public int Id { get; set; }

        [Required(ErrorMessage = "Hãy nhập tên.")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Tên phải có ít nhất 4 và tối đa 100 ký tự.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Hãy nhập địa chỉ email.")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@gmail\.com", ErrorMessage = "Địa chỉ email phải có đuôi gmail.com.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Hãy nhập mật khẩu.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự, bao gồm chữ hoa, chữ thường, số và ký tự đặc biệt.")]
        public string? Password { get; set; }

        public int? Idrole { get; set; }

        public virtual Role? IdroleNavigation { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public Account(string? username, string? email, string? password)
        {
            Username = username;
            Email = email;
            Password = password;
        }
    }
}
