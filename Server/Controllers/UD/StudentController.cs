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
    public class StudentController : BaseController
    {
        public StudentController(OCTOBEROracleContext context, IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache)
            : base(context, httpContextAccessor)
        {
        }

        // Get a single student by StudentId
        [HttpGet]
        [Route("Get/{StudentId}")]
        public async Task<IActionResult> Get(int StudentId)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var result = await _context.Students
                    .Where(s => s.StudentId == StudentId)
                    .Select(s => new StudentDTO
                    {
                        StudentId = s.StudentId,
                        Salutation = s.Salutation,
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        StreetAddress = s.StreetAddress,
                        Zip = s.Zip,
                        Phone = s.Phone,
                        // Additional properties as defined in StudentDTO
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

        // Add a new student
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] StudentDTO studentDto)
        {
            try
            {
                var student = new Student
                {
                    StudentId = studentDto.StudentId,
                    Salutation = studentDto.Salutation,
                    FirstName = studentDto.FirstName,
                    LastName = studentDto.LastName,
                    StreetAddress = studentDto.StreetAddress,
                    Zip = studentDto.Zip,
                    Phone = studentDto.Phone,
                    // Additional properties as defined in StudentDTO
                };

                _context.Students.Add(student);
                await _context.SaveChangesAsync();

                return Ok(student);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // Update a student
        [HttpPut]
        [Route("Update/{StudentId}")]
        public async Task<IActionResult> Update(int StudentId, [FromBody] StudentDTO studentDto)
        {
            try
            {
                var student = await _context.Students
                    .FirstOrDefaultAsync(s => s.StudentId == StudentId);

                if (student == null)
                    return NotFound();

                student.Salutation = studentDto.Salutation;
                student.FirstName = studentDto.FirstName;
                student.LastName = studentDto.LastName;
                student.StreetAddress = studentDto.StreetAddress;
                student.Zip = studentDto.Zip;
                student.Phone = studentDto.Phone;
                // Additional properties as defined in StudentDTO

                _context.Students.Update(student);
                await _context.SaveChangesAsync();

                return Ok(student);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // Delete a student
        [HttpDelete]
        [Route("Delete/{StudentId}")]
        public async Task<IActionResult> Delete(int StudentId)
        {
            try
            {
                var student = await _context.Students
                    .FirstOrDefaultAsync(s => s.StudentId == StudentId);

                if (student == null)
                    return NotFound();

                _context.Students.Remove(student);
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
