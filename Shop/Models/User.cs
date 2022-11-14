using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class User
    {
        [Required, MinLength(2, ErrorMessage = "Minimum lenght is 2")]
        [Display(Name ="Username")]
        public  string UserName { get; set; }

        [Required, EmailAddress]
        public  string Email { get; set; }

        [DataType(DataType.Password), Required, MinLength(4, ErrorMessage = "Minimum lenght is 2")]
        public  string Password { get; set; }
    
      
    }
}
