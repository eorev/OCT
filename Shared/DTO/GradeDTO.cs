﻿using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace OCTOBER.Shared.DTO
{
    public class GradeDTO
    {
        [Required]
        [Precision(8)]
        public int SchoolId { get; set; }

        [Required]
        [Precision(8)]
        public int StudentId { get; set; }

        [Required]
        [Precision(8)]
        public int SectionId { get; set; }

        [Required]
        [StringLength(2)]
        public string GradeTypeCode { get; set; } = null!;

        [Required]
        [Precision(3)]
        public byte GradeCodeOccurrence { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal NumericGrade { get; set; }

        public string? Comments { get; set; }

        [Required]
        [StringLength(30)]
        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        [StringLength(30)]
        public string ModifiedBy { get; set; } = null!;

        public DateTime ModifiedDate { get; set; }
    }
}