using System;
using System.ComponentModel.DataAnnotations;

namespace OCTOBER.Shared.DTO
{
    public class ZipcodeDTO
    {
        [Required]
        [StringLength(5)]
        public string Zip { get; set; } = null!;

        [StringLength(25)]
        public string? City { get; set; }

        [StringLength(2)]
        public string? State { get; set; }

        [Required]
        [StringLength(30)]
        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        [StringLength(30)]
        public string ModifiedBy { get; set; } = null!;

        public DateTime ModifiedDate { get; set; }

    }
}
