using System.ComponentModel.DataAnnotations;
using inxhsofti.Validators; 

namespace inxhsofti.Models 
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Emri i përdoruesit është i detyruar")]
        [MaxLength(50, ErrorMessage = "Emri i përdoruesit nuk mund të kalojë 50 karaktere")]
        [RegularExpression(@"^[^']*$", ErrorMessage = "Emri i përdoruesit nuk mund të përmbajë thonjëza teke")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Fjalëkalimi është i detyruar")]
        [MaxLength(100, ErrorMessage = "Fjalëkalimi nuk mund të kalojë 100 karaktere")]
        [MinLength(8, ErrorMessage = "Fjalëkalimi duhet të ketë të paktën 8 karaktere")]
        [StrongPassword]
        public string Password { get; set; }
    }
}
