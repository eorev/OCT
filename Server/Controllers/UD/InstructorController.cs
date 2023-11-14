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
    public class InstructorController : BaseController
    {
        public InstructorController(OCTOBEROracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {
        }

        [HttpGet]
        [Route("Get/{InstructorId}")]
        public async Task<IActionResult> Get(int InstructorId)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var result = await _context.Instructors
                    .Where(i => i.InstructorId == InstructorId)
                    .Select(i => new InstructorDTO
                    {
                        SchoolId = i.SchoolId,
                        InstructorId = i.InstructorId,
                        Salutation = i.Salutation,
                        FirstName = i.FirstName,
                        LastName = i.LastName,
                        StreetAddress = i.StreetAddress,
                        Zip = i.Zip,
                        Phone = i.Phone,
                        CreatedBy = i.CreatedBy,
                        CreatedDate = i.CreatedDate,
                        ModifiedBy = i.ModifiedBy,
                        ModifiedDate = i.ModifiedDate
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
        public async Task<IActionResult> Add([FromBody] InstructorDTO instructorDto)
        {
            try
            {
                var instructor = new Instructor
                {
                    SchoolId = instructorDto.SchoolId,
                    InstructorId = instructorDto.InstructorId,
                    Salutation = instructorDto.Salutation,
                    FirstName = instructorDto.FirstName,
                    LastName = instructorDto.LastName,
                    StreetAddress = instructorDto.StreetAddress,
                    Zip = instructorDto.Zip,
                    Phone = instructorDto.Phone,
                    CreatedBy = instructorDto.CreatedBy,
                    CreatedDate = instructorDto.CreatedDate,
                    ModifiedBy = instructorDto.ModifiedBy,
                    ModifiedDate = instructorDto.ModifiedDate
                };

                _context.Instructors.Add(instructor);
                await _context.SaveChangesAsync();

                return Ok(instructor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        [Route("Update/{InstructorId}")]
        public async Task<IActionResult> Update(int InstructorId, [FromBody] InstructorDTO instructorDto)
        {
            try
            {
                var instructor = await _context.Instructors
                    .FirstOrDefaultAsync(i => i.InstructorId == InstructorId);

                if (instructor == null)
                    return NotFound();

                instructor.Salutation = instructorDto.Salutation;
                instructor.FirstName = instructorDto.FirstName;
                instructor.LastName = instructorDto.LastName;
                instructor.StreetAddress = instructorDto.StreetAddress;
                instructor.Zip = instructorDto.Zip;
                instructor.Phone = instructorDto.Phone;
                instructor.CreatedBy = instructorDto.CreatedBy;
                instructor.CreatedDate = instructorDto.CreatedDate;
                instructor.ModifiedBy = instructorDto.ModifiedBy;
                instructor.ModifiedDate = instructorDto.ModifiedDate;

                _context.Instructors.Update(instructor);
                await _context.SaveChangesAsync();

                return Ok(instructor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete]
        [Route("Delete/{InstructorId}")]
        public async Task<IActionResult> Delete(int InstructorId)
        {
            try
            {
                var instructor = await _context.Instructors
                    .FirstOrDefaultAsync(i => i.InstructorId == InstructorId);

                if (instructor == null)
                    return NotFound();

                _context.Instructors.Remove(instructor);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
