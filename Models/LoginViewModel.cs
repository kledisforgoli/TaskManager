using System.ComponentModel.DataAnnotations;

namespace inxhsofti.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email është i detyrueshëm")]
        [EmailAddress(ErrorMessage = "Email nuk është në formatin e duhur")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Fjalëkalimi është i detyrueshëm")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}