using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace OCTOBER.Shared.DTO
{
    public class EnrollmentDTO
    {
        [Required]
        [Precision(8)]
        public int StudentId { get; set; }

        [Required]
        [Precision(8)]
        public int SectionId { get; set; }

        public DateTime EnrollDate { get; set; }

        [Precision(3)]
        public byte? FinalGrade { get; set; }

        [Required]
        [StringLength(30)]
        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        [StringLength(30)]
        public string ModifiedBy { get; set; } = null!;

        public DateTime ModifiedDate { get; set; }

        [Required]
        [Precision(8)]
        public int SchoolId { get; set; }

    }
}
