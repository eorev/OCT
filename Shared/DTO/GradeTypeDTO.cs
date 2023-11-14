using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace OCTOBER.Shared.DTO
{
    public class GradeTypeDTO
    {
        [Required]
        [Precision(8)]
        public int SchoolId { get; set; }

        [Required]
        [StringLength(2)]
        public string GradeTypeCode { get; set; } = null!;

        [StringLength(50)]
        public string Description { get; set; } = null!;

        [StringLength(30)]
        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        [StringLength(30)]
        public string ModifiedBy { get; set; } = null!;

        public DateTime ModifiedDate { get; set; }
    }
}
