using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OCTOBER.EF.Data; // Replace with your actual data context namespace
using OCTOBER.EF.Models; // Replace with your actual model namespace for Section
using OCTOBER.Shared.DTO; // Namespace where SectionDTO is located
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
    public class SectionController : BaseController
    {
        public SectionController(OCTOBEROracleContext context, IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache)
            : base(context, httpContextAccessor)
        {
        }

        // Get a single section by ID
        [HttpGet]
        [Route("Get/{SectionId}")]
        public async Task<IActionResult> Get(int SectionId)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var result = await _context.Sections
                    .Where(x => x.SectionId == SectionId)
                    .Select(sp => new SectionDTO
                    {
                        SectionId = sp.SectionId,
                        CourseNo = sp.CourseNo,
                        SectionNo = sp.SectionNo,
                        StartDateTime = sp.StartDateTime,
                        Location = sp.Location,
                        InstructorId = sp.InstructorId,
                        Capacity = sp.Capacity,
                        CreatedBy = sp.CreatedBy,
                        CreatedDate = sp.CreatedDate,
                        ModifiedBy = sp.ModifiedBy,
                        ModifiedDate = sp.ModifiedDate
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

        // Get all sections
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var result = await _context.Sections.Select(sp => new SectionDTO
                {
                    SectionId = sp.SectionId,
                    CourseNo = sp.CourseNo,
                    SectionNo = sp.SectionNo,
                    StartDateTime = sp.StartDateTime,
                    Location = sp.Location,
                    InstructorId = sp.InstructorId,
                    Capacity = sp.Capacity,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate
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

        // Create a new section
        [HttpPost]
        [Route("Post")]
        public async Task<IActionResult> Post([FromBody] SectionDTO sectionDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                Section section = new Section
                {
                    SectionId = sectionDTO.SectionId,
                    CourseNo = sectionDTO.CourseNo,
                    SectionNo = sectionDTO.SectionNo,
                    StartDateTime = sectionDTO.StartDateTime,
                    Location = sectionDTO.Location,
                    InstructorId = sectionDTO.InstructorId,
                    Capacity = sectionDTO.Capacity,
                    CreatedBy = sectionDTO.CreatedBy,
                    CreatedDate = sectionDTO.CreatedDate,
                    ModifiedBy = sectionDTO.ModifiedBy,
                    ModifiedDate = sectionDTO.ModifiedDate
                };

                _context.Sections.Add(section);
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

        // Update an existing section
        [HttpPut]
        [Route("Put")]
        public async Task<IActionResult> Put([FromBody] SectionDTO sectionDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var section = await _context.Sections.Where(x => x.SectionId == sectionDTO.SectionId).FirstOrDefaultAsync();
                if (section == null)
                {
                    return NotFound();
                }

                section.CourseNo = sectionDTO.CourseNo;
                section.SectionNo = sectionDTO.SectionNo;
                section.StartDateTime = sectionDTO.StartDateTime;
                section.Location = sectionDTO.Location;
                section.InstructorId = sectionDTO.InstructorId;
                section.Capacity = sectionDTO.Capacity;
                section.CreatedBy = sectionDTO.CreatedBy;
                section.CreatedDate = sectionDTO.CreatedDate;
                section.ModifiedBy = sectionDTO.ModifiedBy;
                section.ModifiedDate = sectionDTO.ModifiedDate;

                _context.Sections.Update(section);
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

        // Delete a section
        [HttpDelete]
        [Route("Delete/{SectionId}")]
        public async Task<IActionResult> Delete(int SectionId)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var section = await _context.Sections.Where(x => x.SectionId == SectionId).FirstOrDefaultAsync();
                if (section != null)
                {
                    _context.Sections.Remove(section);
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