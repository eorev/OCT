using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace OCTOBER.Shared.DTO
{
    public class GradeTypeWeightDTO
    {
        [Required]
        [Precision(8)]
        public int SchoolId { get; set; }

        [Required]
        [Precision(8)]
        public int SectionId { get; set; }

        [Required]
        [StringLength(2)]
        public string GradeTypeCode { get; set; } = null!;

        [Required]
        [Precision(3)]
        public byte NumberPerSection { get; set; }

        [Required]
        [Precision(3)]
        public byte PercentOfFinalGrade { get; set; }

        [Required]
        public bool DropLowest { get; set; }

        [Required]
        [StringLength(30)]
        [Unicode(false)]
        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        [StringLength(30)]
        [Unicode(false)]
        public string ModifiedBy { get; set; } = null!;

        public DateTime ModifiedDate { get; set; }
    }
}
