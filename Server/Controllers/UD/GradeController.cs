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

namespace OCTOBER.Server.Controllers.UD
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeController : BaseController
    {
        public GradeController(OCTOBEROracleContext context, IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache)
            : base(context, httpContextAccessor)
        {
        }

        // Get a single grade by StudentId and SectionId
        [HttpGet]
        [Route("Get/{StudentId}/{SectionId}")]
        public async Task<IActionResult> Get(int StudentId, int SectionId)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var result = await _context.Grades
                    .Where(g => g.StudentId == StudentId && g.SectionId == SectionId)
                    .Select(g => new GradeDTO
                    {
                        SchoolId = g.SchoolId,
                        StudentId = g.StudentId,
                        SectionId = g.SectionId,
                        GradeTypeCode = g.GradeTypeCode,
                        GradeCodeOccurrence = g.GradeCodeOccurrence,
                        NumericGrade = g.NumericGrade,
                        Comments = g.Comments,
                        CreatedBy = g.CreatedBy,
                        CreatedDate = g.CreatedDate,
                        ModifiedBy = g.ModifiedBy,
                        ModifiedDate = g.ModifiedDate
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

        // Add a new grade
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] GradeDTO gradeDto)
        {
            try
            {
                var grade = new Grade
                {
                    SchoolId = gradeDto.SchoolId,
                    StudentId = gradeDto.StudentId,
                    SectionId = gradeDto.SectionId,
                    GradeTypeCode = gradeDto.GradeTypeCode,
                    GradeCodeOccurrence = gradeDto.GradeCodeOccurrence,
                    NumericGrade = gradeDto.NumericGrade,
                    Comments = gradeDto.Comments,
                    CreatedBy = gradeDto.CreatedBy,
                    CreatedDate = gradeDto.CreatedDate,
                    ModifiedBy = gradeDto.ModifiedBy,
                    ModifiedDate = gradeDto.ModifiedDate
                };

                _context.Grades.Add(grade);
                await _context.SaveChangesAsync();

                return Ok(grade);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // Update a grade
        [HttpPut]
        [Route("Update/{StudentId}/{SectionId}")]
        public async Task<IActionResult> Update(int StudentId, int SectionId, [FromBody] GradeDTO gradeDto)
        {
            try
            {
                var grade = await _context.Grades
                    .FirstOrDefaultAsync(g => g.StudentId == StudentId && g.SectionId == SectionId);

                if (grade == null)
                    return NotFound();

                grade.SchoolId = gradeDto.SchoolId;
                grade.GradeTypeCode = gradeDto.GradeTypeCode;
                grade.GradeCodeOccurrence = gradeDto.GradeCodeOccurrence;
                grade.NumericGrade = gradeDto.NumericGrade;
                grade.Comments = gradeDto.Comments;
                grade.CreatedBy = gradeDto.CreatedBy;
                grade.CreatedDate = gradeDto.CreatedDate;
                grade.ModifiedBy = gradeDto.ModifiedBy;
                grade.ModifiedDate = gradeDto.ModifiedDate;

                _context.Grades.Update(grade);
                await _context.SaveChangesAsync();

                return Ok(grade);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // Delete a grade
        [HttpDelete]
        [Route("Delete/{StudentId}/{SectionId}")]
        public async Task<IActionResult> Delete(int StudentId, int SectionId)
        {
            try
            {
                var grade = await _context.Grades
                    .FirstOrDefaultAsync(g => g.StudentId == StudentId && g.SectionId == SectionId);

                if (grade == null)
                    return NotFound();

                _context.Grades.Remove(grade);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
