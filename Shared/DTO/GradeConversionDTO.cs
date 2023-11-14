using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace OCTOBER.Shared.DTO
{
    public class GradeConversionDTO
    {
        [Required]
        [Precision(8)]
        public int SchoolId { get; set; }

        [Required]
        [StringLength(2)]
        public string LetterGrade { get; set; } = null!;

        [Required]
        [Range(0, 4.0, ErrorMessage = "Grade point must be between 0 and 4.0")]
        public decimal GradePoint { get; set; }

        [Required]
        [Precision(3)]
        public byte MaxGrade { get; set; }

        [Required]
        [Precision(3)]
        public byte MinGrade { get; set; }

        [Required]
        [StringLength(30)]
        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        [StringLength(30)]
        public string ModifiedBy { get; set; } = null!;

        public DateTime ModifiedDate { get; set; }
    }
}
