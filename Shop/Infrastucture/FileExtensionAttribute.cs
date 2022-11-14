using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace Shop.Infrastucture
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //var context = (ShopContext)validationContext.GetService(typeof(ShopContext));

            var file = value as IFormFile;
            if(file != null)
            {
                var extension = Path.GetExtension(file.FileName);

                string[] extensions = { "jpg", "png" };
                bool result = extension.Any(x => extension.EndsWith(x));

                if(!result)
                {
                    return new ValidationResult(GetErrorMessage());
                }
                
            }
            return ValidationResult.Success;
        }

        private string GetErrorMessage()
        {
            return "Allowed extension are jpg and png.";
        }
    }
}
