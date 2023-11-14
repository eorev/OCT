using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OCTOBER.EF.Data;
using OCTOBER.EF.Models;
using OCTOBER.Shared.DTO;
using System.Linq.Dynamic.Core;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using OCTOBER.Server.Controllers.Base;
using System;

namespace OCTOBER.Server.Controllers.UD
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeTypeWeightController : BaseController
    {
        public GradeTypeWeightController(OCTOBEROracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {
        }

        [HttpGet]
        [Route("Get/{SchoolId}/{SectionId}/{GradeTypeCode}")]
        public async Task<IActionResult> Get(int SchoolId, int SectionId, string GradeTypeCode)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var result = await _context.GradeTypeWeights
                    .Where(gtw => gtw.SchoolId == SchoolId && gtw.SectionId == SectionId && gtw.GradeTypeCode == GradeTypeCode)
                    .Select(gtw => new GradeTypeWeightDTO
                    {
                        SchoolId = gtw.SchoolId,
                        SectionId = gtw.SectionId,
                        GradeTypeCode = gtw.GradeTypeCode,
                        NumberPerSection = gtw.NumberPerSection,
                        PercentOfFinalGrade = gtw.PercentOfFinalGrade,
                        DropLowest = gtw.DropLowest,
                        CreatedBy = gtw.CreatedBy,
                        CreatedDate = gtw.CreatedDate,
                        ModifiedBy = gtw.ModifiedBy,
                        ModifiedDate = gtw.ModifiedDate
                    })
                    .FirstOrDefaultAsync();

                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] GradeTypeWeightDTO gradeTypeWeightDto)
        {
            try
            {
                var gradeTypeWeight = new GradeTypeWeight
                {
                    SchoolId = gradeTypeWeightDto.SchoolId,
                    SectionId = gradeTypeWeightDto.SectionId,
                    GradeTypeCode = gradeTypeWeightDto.GradeTypeCode,
                    NumberPerSection = gradeTypeWeightDto.NumberPerSection,
                    PercentOfFinalGrade = gradeTypeWeightDto.PercentOfFinalGrade,
                    DropLowest = gradeTypeWeightDto.DropLowest,
                    CreatedBy = gradeTypeWeightDto.CreatedBy,
                    CreatedDate = gradeTypeWeightDto.CreatedDate,
                    ModifiedBy = gradeTypeWeightDto.ModifiedBy,
                    ModifiedDate = gradeTypeWeightDto.ModifiedDate
                };

                _context.GradeTypeWeights.Add(gradeTypeWeight);
                await _context.SaveChangesAsync();

                return Ok(gradeTypeWeight);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        [Route("Update/{SchoolId}/{SectionId}/{GradeTypeCode}")]
        public async Task<IActionResult> Update(int SchoolId, int SectionId, string GradeTypeCode, [FromBody] GradeTypeWeightDTO gradeTypeWeightDto)
        {
            try
            {
                var gradeTypeWeight = await _context.GradeTypeWeights
                    .FirstOrDefaultAsync(gtw => gtw.SchoolId == SchoolId && gtw.SectionId == SectionId && gtw.GradeTypeCode == GradeTypeCode);

                if (gradeTypeWeight == null)
                    return NotFound();

                gradeTypeWeight.NumberPerSection = gradeTypeWeightDto.NumberPerSection;
                gradeTypeWeight.PercentOfFinalGrade = gradeTypeWeightDto.PercentOfFinalGrade;
                gradeTypeWeight.DropLowest = gradeTypeWeightDto.DropLowest;
                gradeTypeWeight.CreatedBy = gradeTypeWeightDto.CreatedBy;
                gradeTypeWeight.CreatedDate = gradeTypeWeightDto.CreatedDate;
                gradeTypeWeight.ModifiedBy = gradeTypeWeightDto.ModifiedBy;
                gradeTypeWeight.ModifiedDate = gradeTypeWeightDto.ModifiedDate;

                _context.GradeTypeWeights.Update(gradeTypeWeight);
                await _context.SaveChangesAsync();

                return Ok(gradeTypeWeight);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete]
        [Route("Delete/{SchoolId}/{SectionId}/{GradeTypeCode}")]
        public async Task<IActionResult> Delete(int SchoolId, int SectionId, string GradeTypeCode)
        {
            var gradeTypeWeight = await _context.GradeTypeWeights
    .FirstOrDefaultAsync(gtw => gtw.SchoolId == SchoolId && gtw.SectionId == SectionId && gtw.GradeTypeCode == GradeTypeCode);

            if (gradeTypeWeight == null)
                return NotFound();

            _context.GradeTypeWeights.Remove(gradeTypeWeight);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}