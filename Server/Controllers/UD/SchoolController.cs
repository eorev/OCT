﻿using Microsoft.AspNetCore.Mvc;
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
    public class SchoolController : BaseController
    {
        public SchoolController(OCTOBEROracleContext context, IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache)
            : base(context, httpContextAccessor)
        {
        }

        // Get a single school by ID
        [HttpGet]
        [Route("Get/{SchoolId}")]
        public async Task<IActionResult> Get(int SchoolId)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var result = await _context.Schools
                    .Where(x => x.SchoolId == SchoolId)
                    .Select(sp => new SchoolDTO
                    {
                        SchoolId = sp.SchoolId,
                        SchoolName = sp.SchoolName,
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

        // Get all schools
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var result = await _context.Schools.Select(sp => new SchoolDTO
                {
                    SchoolId = sp.SchoolId,
                    SchoolName = sp.SchoolName,
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

        // Create a new school
        [HttpPost]
        [Route("Post")]
        public async Task<IActionResult> Post([FromBody] SchoolDTO schoolDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                School school = new School
                {
                    SchoolId = schoolDTO.SchoolId,
                    SchoolName = schoolDTO.SchoolName,
                    CreatedBy = schoolDTO.CreatedBy,
                    CreatedDate = schoolDTO.CreatedDate,
                    ModifiedBy = schoolDTO.ModifiedBy,
                    ModifiedDate = schoolDTO.ModifiedDate
                };

                _context.Schools.Add(school);
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

        // Update an existing school
        [HttpPut]
        [Route("Put")]
        public async Task<IActionResult> Put([FromBody] SchoolDTO schoolDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var school = await _context.Schools.Where(x => x.SchoolId == schoolDTO.SchoolId).FirstOrDefaultAsync();
                if (school == null)
                {
                    return NotFound();
                }

                school.SchoolId = schoolDTO.SchoolId;
                school.SchoolName = schoolDTO.SchoolName;
                school.CreatedBy = schoolDTO.CreatedBy;
                school.CreatedDate = schoolDTO.CreatedDate;
                school.ModifiedBy = schoolDTO.ModifiedBy;
                school.ModifiedDate = schoolDTO.ModifiedDate;

                _context.Schools.Update(school);
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

        // Delete a school
        [HttpDelete]
        [Route("Delete/{SchoolId}")]
        public async Task<IActionResult> Delete(int SchoolId)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var school = await _context.Schools.Where(x => x.SchoolId == SchoolId).FirstOrDefaultAsync();
                if (school != null)
                {
                    _context.Schools.Remove(school);
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