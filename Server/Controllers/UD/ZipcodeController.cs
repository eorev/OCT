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
    public class ZipcodeController : BaseController
    {
        public ZipcodeController(OCTOBEROracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {
        }

        [HttpGet]
        [Route("Get/{Zip}")]
        public async Task<IActionResult> Get(string Zip)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var result = await _context.Zipcodes
                    .Where(z => z.Zip == Zip)
                    .Select(z => new ZipcodeDTO
                    {
                        Zip = z.Zip,
                        City = z.City,
                        State = z.State,
                        CreatedBy = z.CreatedBy,
                        CreatedDate = z.CreatedDate,
                        ModifiedBy = z.ModifiedBy,
                        ModifiedDate = z.ModifiedDate
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
        public async Task<IActionResult> Add([FromBody] ZipcodeDTO zipcodeDto)
        {
            try
            {
                var zipcode = new Zipcode
                {
                    Zip = zipcodeDto.Zip,
                    City = zipcodeDto.City,
                    State = zipcodeDto.State,
                    CreatedBy = zipcodeDto.CreatedBy,
                    CreatedDate = zipcodeDto.CreatedDate,
                    ModifiedBy = zipcodeDto.ModifiedBy,
                    ModifiedDate = zipcodeDto.ModifiedDate
                };

                _context.Zipcodes.Add(zipcode);
                await _context.SaveChangesAsync();

                return Ok(zipcode);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        [Route("Update/{Zip}")]
        public async Task<IActionResult> Update(string Zip, [FromBody] ZipcodeDTO zipcodeDto)
        {
            try
            {
                var zipcode = await _context.Zipcodes
                    .FirstOrDefaultAsync(z => z.Zip == Zip);

                if (zipcode == null)
                    return NotFound();

                zipcode.City = zipcodeDto.City;
                zipcode.State = zipcodeDto.State;
                zipcode.CreatedBy = zipcodeDto.CreatedBy;
                zipcode.CreatedDate = zipcodeDto.CreatedDate;
                zipcode.ModifiedBy = zipcodeDto.ModifiedBy;
                zipcode.ModifiedDate = zipcodeDto.ModifiedDate;

                _context.Zipcodes.Update(zipcode);
                await _context.SaveChangesAsync();

                return Ok(zipcode);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete]
        [Route("Delete/{Zip}")]
        public async Task<IActionResult> Delete(string Zip)
        {
            try
            {
                var zipcode = await _context.Zipcodes
                    .FirstOrDefaultAsync(z => z.Zip == Zip);

                if (zipcode == null)
                    return NotFound();

                _context.Zipcodes.Remove(zipcode);
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
