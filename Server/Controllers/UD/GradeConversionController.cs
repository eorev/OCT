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
    public class GradeConversionController : BaseController
    {
        public GradeConversionController(OCTOBEROracleContext context, IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache)
            : base(context, httpContextAccessor)
        {
        }

        // Get a single grade conversion by SchoolId and LetterGrade
        [HttpGet]
        [Route("Get/{SchoolId}/{LetterGrade}")]
        public async Task<IActionResult> Get(int SchoolId, string LetterGrade)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var result = await _context.GradeConversions
                    .Where(x => x.SchoolId == SchoolId && x.LetterGrade == LetterGrade)
                    .Select(gc => new GradeConversionDTO
                    {
                        SchoolId = gc.SchoolId,
                        LetterGrade = gc.LetterGrade,
                        GradePoint = gc.GradePoint,
                        MaxGrade = gc.MaxGrade,
                        MinGrade = gc.MinGrade,
                        CreatedBy = gc.CreatedBy,
                        CreatedDate = gc.CreatedDate,
                        ModifiedBy = gc.ModifiedBy,
                        ModifiedDate = gc.ModifiedDate
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

        // Get all grade conversions for a school
        [HttpGet]
        [Route("Get/{SchoolId}")]
        public async Task<IActionResult> Get(int SchoolId)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var result = await _context.GradeConversions
                    .Where(x => x.SchoolId == SchoolId)
                    .Select(gc => new GradeConversionDTO
                    {
                        SchoolId = gc.SchoolId,
                        LetterGrade = gc.LetterGrade,
                        GradePoint = gc.GradePoint,
                        MaxGrade = gc.MaxGrade,
                        MinGrade = gc.MinGrade,
                        CreatedBy = gc.CreatedBy,
                        CreatedDate = gc.CreatedDate,
                        ModifiedBy = gc.ModifiedBy,
                        ModifiedDate = gc.ModifiedDate
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

        // Create a new grade conversion
        [HttpPost]
        [Route("Post")]
        public async Task<IActionResult> Post([FromBody] GradeConversionDTO gradeConversionDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                GradeConversion gradeConversion = new GradeConversion
                {
                    SchoolId = gradeConversionDTO.SchoolId,
                    LetterGrade = gradeConversionDTO.LetterGrade,
                    GradePoint = gradeConversionDTO.GradePoint,
                    MaxGrade = gradeConversionDTO.MaxGrade,
                    MinGrade = gradeConversionDTO.MinGrade,
                    CreatedBy = gradeConversionDTO.CreatedBy,
                    CreatedDate = gradeConversionDTO.CreatedDate,
                    ModifiedBy = gradeConversionDTO.ModifiedBy,
                    ModifiedDate = gradeConversionDTO.ModifiedDate
                };

                _context.GradeConversions.Add(gradeConversion);
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

        // Update an existing grade conversion
        [HttpPut]
        [Route("Put")]
        public async Task<IActionResult> Put([FromBody] GradeConversionDTO gradeConversionDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var gradeConversion = await _context.GradeConversions
                    .Where(x => x.SchoolId == gradeConversionDTO.SchoolId && x.LetterGrade == gradeConversionDTO.LetterGrade)
                    .FirstOrDefaultAsync();
                if (gradeConversion == null)
                {
                    return NotFound();
                }

                gradeConversion.GradePoint = gradeConversionDTO.GradePoint;
                gradeConversion.MaxGrade = gradeConversionDTO.MaxGrade;
                gradeConversion.MinGrade = gradeConversionDTO.MinGrade;
                gradeConversion.CreatedBy = gradeConversionDTO.CreatedBy;
                gradeConversion.CreatedDate = gradeConversionDTO.CreatedDate;
                gradeConversion.ModifiedBy = gradeConversionDTO.ModifiedBy;
                gradeConversion.ModifiedDate = gradeConversionDTO.ModifiedDate;

                _context.GradeConversions.Update(gradeConversion);
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

        // Delete a grade conversion
        [HttpDelete]
        [Route("Delete/{SchoolId}/{LetterGrade}")]
        public async Task<IActionResult> Delete(int SchoolId, string LetterGrade)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var gradeConversion = await _context.GradeConversions
                    .Where(x => x.SchoolId == SchoolId && x.LetterGrade == LetterGrade)
                    .FirstOrDefaultAsync();
                if (gradeConversion != null)
                {
                    _context.GradeConversions.Remove(gradeConversion);
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