using System.ComponentModel.DataAnnotations;

namespace ShopQuanAo.Models
{
    public partial class LoginCredential
    {
        public LoginCredential()
        {
        }
        public LoginCredential(string? email, string? password)
        {
            Email = email;
            Password = password;
        }

        [Required(ErrorMessage = "Hãy nhập địa chỉ email.")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@gmail\.com", ErrorMessage = "Địa chỉ email phải có đuôi gmail.com.")]
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
