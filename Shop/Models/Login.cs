using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Shop.Models
{
    public class Login
    {
        
        [Required, EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password), Required, MinLength(4, ErrorMessage = "Minimum lenght is 4")]
        public string Password { get; set; }

        public string ReturnUrl  { get; set; }
      
    }
}
