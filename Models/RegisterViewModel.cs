using System.ComponentModel.DataAnnotations;
using inxhsofti.Validators; 

namespace inxhsofti.Models 
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email-i është i detyruar")]
        [EmailAddress(ErrorMessage = "Ju lutemi vendosni një adresë email të vlefshme")]
        [MaxLength(50, ErrorMessage = "Email-i nuk mund të kalojë 50 karaktere")]
        [RegularExpression(@"^[^'--]*$", ErrorMessage = "Email-i nuk mund të përmbajë thonjëza teke ose --")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Fjalëkalimi është i detyruar")]
        [MinLength(8, ErrorMessage = "Fjalëkalimi duhet të ketë të paktën 8 karaktere")]
        [MaxLength(100, ErrorMessage = "Fjalëkalimi nuk mund të kalojë 100 karaktere")]
        [RegularExpression(@"^[^']*$", ErrorMessage = "Fjalëkalimi nuk mund të përmbajë thonjëza teke")]
        [StrongPassword(ErrorMessage = "Fjalëkalimi duhet të përmbajë të paktën një shkronjë të madhe, një numër dhe një karakter special")]
        [Display(Name = "Fjalëkalim")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Konfirmimi i fjalëkalimit është i detyruar")]
        [Compare("Password", ErrorMessage = "Fjalëkalimet nuk përputhen")]
        [Display(Name = "Konfirmo fjalëkalimin")]
        public string ConfirmPassword { get; set; }
    }
}
