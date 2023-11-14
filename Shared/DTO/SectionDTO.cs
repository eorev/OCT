using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace OCTOBER.Shared.DTO
{
    public class SectionDTO
    {
        [Required]
        [Precision(8)]
        public int SectionId { get; set; }

        [Required]
        [Precision(8)]
        public int CourseNo { get; set; }

        [Required]
        [Precision(3)]
        public byte SectionNo { get; set; }

        public DateTime? StartDateTime { get; set; }

        [StringLength(50)]
        public string? Location { get; set; }

        [Required]
        [Precision(8)]
        public int InstructorId { get; set; }

        [Precision(3)]
        public byte? Capacity { get; set; }

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
