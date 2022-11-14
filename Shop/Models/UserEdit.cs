using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class UserEdit
    {
       
        [Required, EmailAddress]
        public  string Email { get; set; }
        [DataType(DataType.Password), MinLength(4, ErrorMessage = "Minimum lenght is 2")]
        public  string Password { get; set; }
        public UserEdit()
        {
          
        }
        public UserEdit(AppUser appUser)
        {      
            Email = appUser.Email;
            Password = appUser.PasswordHash;
        }
    }
}
