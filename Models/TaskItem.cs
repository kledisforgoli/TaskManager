using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace inxhsofti.Models
{
    public class TaskItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Titulli është i detyrueshëm")]
        [StringLength(100, ErrorMessage = "Titulli nuk mund të jetë më i gjatë se 100 karaktere")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "Përshkrimi nuk mund të jetë më i gjatë se 500 karaktere")]
        public string? Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Afati")]
        public DateTime? DueDate { get; set; }

        [Display(Name = "E përfunduar")]
        public bool IsCompleted { get; set; } = false;



        [ForeignKey("User")]
        public int UserId { get; set; }

        [ValidateNever]
        public virtual User User { get; set; }
    }
}