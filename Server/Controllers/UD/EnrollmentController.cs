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
    public class EnrollmentController : BaseController
    {
        public EnrollmentController(OCTOBEROracleContext context, IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache)
            : base(context, httpContextAccessor)
        {
        }

        // Get a single enrollment by StudentId and SectionId
        [HttpGet]
        [Route("Get/{StudentId}/{SectionId}")]
        public async Task<IActionResult> Get(int StudentId, int SectionId)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var result = await _context.Enrollments
                    .Where(x => x.StudentId == StudentId && x.SectionId == SectionId)
                    .Select(en => new EnrollmentDTO
                    {
                        StudentId = en.StudentId,
                        SectionId = en.SectionId,
                        EnrollDate = en.EnrollDate,
                        FinalGrade = en.FinalGrade,
                        CreatedBy = en.CreatedBy,
                        CreatedDate = en.CreatedDate,
                        ModifiedBy = en.ModifiedBy,
                        ModifiedDate = en.ModifiedDate,
                        SchoolId = en.SchoolId
                    })
                    .SingleOrDefaultAsync();

                await _context.Database.RollbackTransactionAsync();
                return Ok(result);
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }

        // Get all enrollments
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var result = await _context.Enrollments.Select(en => new EnrollmentDTO
                {
                    StudentId = en.StudentId,
                    SectionId = en.SectionId,
                    EnrollDate = en.EnrollDate,
                    FinalGrade = en.FinalGrade,
                    CreatedBy = en.CreatedBy,
                    CreatedDate = en.CreatedDate,
                    ModifiedBy = en.ModifiedBy,
                    ModifiedDate = en.ModifiedDate,
                    SchoolId = en.SchoolId
                })
                .ToListAsync();

                await _context.Database.RollbackTransactionAsync();
                return Ok(result);
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }

        // Create a new enrollment
        [HttpPost]
        [Route("Post")]
        public async Task<IActionResult> Post([FromBody] EnrollmentDTO enrollmentDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                Enrollment enrollment = new Enrollment
                {
                    StudentId = enrollmentDTO.StudentId,
                    SectionId = enrollmentDTO.SectionId,
                    EnrollDate = enrollmentDTO.EnrollDate,
                    FinalGrade = enrollmentDTO.FinalGrade,
                    CreatedBy = enrollmentDTO.CreatedBy,
                    CreatedDate = enrollmentDTO.CreatedDate,
                    ModifiedBy = enrollmentDTO.ModifiedBy,
                    ModifiedDate = enrollmentDTO.ModifiedDate,
                    SchoolId = enrollmentDTO.SchoolId
                };

                _context.Enrollments.Add(enrollment);
                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();

                return Ok();
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }

        // Update an existing enrollment
        [HttpPut]
        [Route("Put")]
        public async Task<IActionResult> Put([FromBody] EnrollmentDTO enrollmentDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var enrollment = await _context.Enrollments
                    .Where(x => x.StudentId == enrollmentDTO.StudentId && x.SectionId == enrollmentDTO.SectionId)
                    .FirstOrDefaultAsync();
                if (enrollment == null)
                {
                    return NotFound();
                }

                enrollment.EnrollDate = enrollmentDTO.EnrollDate;
                enrollment.FinalGrade = enrollmentDTO.FinalGrade;
                enrollment.CreatedBy = enrollmentDTO.CreatedBy;
                enrollment.CreatedBy = enrollmentDTO.CreatedBy;
                enrollment.CreatedDate = enrollmentDTO.CreatedDate;
                enrollment.ModifiedBy = enrollmentDTO.ModifiedBy;
                enrollment.ModifiedDate = enrollmentDTO.ModifiedDate;
                enrollment.SchoolId = enrollmentDTO.SchoolId;

                _context.Enrollments.Update(enrollment);
                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();

                return Ok();
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }

        // Delete an enrollment
        [HttpDelete]
        [Route("Delete/{StudentId}/{SectionId}")]
        public async Task<IActionResult> Delete(int StudentId, int SectionId)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var enrollment = await _context.Enrollments
                    .Where(x => x.StudentId == StudentId && x.SectionId == SectionId)
                    .FirstOrDefaultAsync();
                if (enrollment != null)
                {
                    _context.Enrollments.Remove(enrollment);
                    await _context.SaveChangesAsync();
                    await _context.Database.CommitTransactionAsync();
                }

                return Ok();
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }
    }
}