using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BlogSinhVien.Models
{
	public class EmailEduValidation: ValidationAttribute
    {
        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {

            Regex rgx = new Regex(@"^[0-9]{10}@sv.hcmunre.edu.vn");
            if (!rgx.IsMatch(value.ToString()))
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"Nhập đúng cú pháp [0-9]@sv.hcmunre.edu.vn";
        }
    }
}
