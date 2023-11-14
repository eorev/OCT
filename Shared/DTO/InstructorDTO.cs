using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace OCTOBER.Shared.DTO
{
    public class InstructorDTO
    {
        [Required]
        [Precision(8)]
        public int SchoolId { get; set; }

        [Required]
        [Precision(8)]
        public int InstructorId { get; set; }

        [StringLength(5)]
        public string? Salutation { get; set; }

        [Required]
        [StringLength(25)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(25)]
        public string LastName { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string StreetAddress { get; set; } = null!;

        [Required]
        [StringLength(5)]
        public string Zip { get; set; } = null!;

        [StringLength(15)]
        public string? Phone { get; set; }

        [Required]
        [StringLength(30)]
        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        [StringLength(30)]
        public string ModifiedBy { get; set; } = null!;

        public DateTime ModifiedDate { get; set; }
    }
}
