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
    public class GradeTypeController : BaseController
    {
        public GradeTypeController(OCTOBEROracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {
        }

        [HttpGet]
        [Route("Get/{GradeTypeCode}")]
        public async Task<IActionResult> Get(string GradeTypeCode)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var result = await _context.GradeTypes
                    .Where(gt => gt.GradeTypeCode == GradeTypeCode)
                    .Select(gt => new GradeTypeDTO
                    {
                        SchoolId = gt.SchoolId,
                        GradeTypeCode = gt.GradeTypeCode,
                        Description = gt.Description,
                        CreatedBy = gt.CreatedBy,
                        CreatedDate = gt.CreatedDate,
                        ModifiedBy = gt.ModifiedBy,
                        ModifiedDate = gt.ModifiedDate
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
        public async Task<IActionResult> Add([FromBody] GradeTypeDTO gradeTypeDto)
        {
            try
            {
                var gradeType = new GradeType
                {
                    SchoolId = gradeTypeDto.SchoolId,
                    GradeTypeCode = gradeTypeDto.GradeTypeCode,
                    Description = gradeTypeDto.Description,
                    CreatedBy = gradeTypeDto.CreatedBy,
                    CreatedDate = gradeTypeDto.CreatedDate,
                    ModifiedBy = gradeTypeDto.ModifiedBy,
                    ModifiedDate = gradeTypeDto.ModifiedDate
                };

                _context.GradeTypes.Add(gradeType);
                await _context.SaveChangesAsync();

                return Ok(gradeType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        [Route("Update/{GradeTypeCode}")]
        public async Task<IActionResult> Update(string GradeTypeCode, [FromBody] GradeTypeDTO gradeTypeDto)
        {
            try
            {
                var gradeType = await _context.GradeTypes
                    .FirstOrDefaultAsync(gt => gt.GradeTypeCode == GradeTypeCode);

                if (gradeType == null)
                    return NotFound();

                gradeType.SchoolId = gradeTypeDto.SchoolId;
                gradeType.Description = gradeTypeDto.Description;
                gradeType.CreatedBy = gradeTypeDto.CreatedBy;
                gradeType.CreatedDate = gradeTypeDto.CreatedDate;
                gradeType.ModifiedBy = gradeTypeDto.ModifiedBy;
                gradeType.ModifiedDate = gradeTypeDto.ModifiedDate;

                _context.GradeTypes.Update(gradeType);
                await _context.SaveChangesAsync();

                return Ok(gradeType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete]
        [Route("Delete/{GradeTypeCode}")]
        public async Task<IActionResult> Delete(string GradeTypeCode)
        {
            try
            {
                var gradeType = await _context.GradeTypes
                    .FirstOrDefaultAsync(gt => gt.GradeTypeCode == GradeTypeCode);

                if (gradeType == null)
                    return NotFound();

                _context.GradeTypes.Remove(gradeType);
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
