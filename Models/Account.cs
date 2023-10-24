using System;
using System.Collections.Generic;

namespace ShopQuanAo.Models
{
    public partial class Account
    {
        public Account()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
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
