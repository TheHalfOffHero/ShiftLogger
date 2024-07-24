using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShiftLogger.Models;
using ShiftLogger.Services;

namespace ShiftLogger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftController : ControllerBase
    {
        private readonly ShiftLoggerContext _context;
        //private readonly ShiftService _shiftService;

        public ShiftController(ShiftLoggerContext context)
        {
            _context = context;
            //_shiftService = service;
        }

        [HttpPost("startshift/{employeeName}")]
        public async Task<ActionResult<Shift>> StartShift(string employeeName)
        {
            ShiftService _shiftService = new ShiftService(_context);
            int latestShiftId = _shiftService.GetNextShiftId();
            var result = await _shiftService.StartShift(employeeName, latestShiftId);
            return result;
        }

        [HttpPut("endshift/{id}")]
        public async Task<ActionResult<Shift>> EndShift(int id)
        {
            ShiftService _shiftService = new ShiftService(_context);
            var result = await _shiftService.EndShift(id);
            return result;
        }
        
        // GET: api/Shift
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shift>>> GetShifts()
        {
            return await _context.Shifts.ToListAsync();
        }
        
        // GET: api/Shift/5
        // [HttpGet("{id}")]
        // public async Task<ActionResult<Shift>> GetShift(int id)
        // {
        //     var shift = await _context.Shifts.FindAsync(id);
        //
        //     if (shift == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     return shift;
        // }
        
        // // PUT: api/Shift/5
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutShift(int id, Shift shift)
        // {
        //     if (id != shift.Id)
        //     {
        //         return BadRequest();
        //     }
        //
        //     _context.Entry(shift).State = EntityState.Modified;
        //
        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!ShiftExists(id))
        //         {
        //             return NotFound();
        //         }
        //         else
        //         {
        //             throw;
        //         }
        //     }
        //
        //     return NoContent();
        // }
        
        // POST: api/Shift
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPost]
        // public async Task<ActionResult<Shift>> PostShift(Shift shift)
        // {
        //     _context.Shifts.Add(shift);
        //     await _context.SaveChangesAsync();
        //
        //     return CreatedAtAction("GetShift", new { id = shift.Id }, shift);
        // }
        
        // DELETE: api/Shift/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShift(int id)
        {
            var shift = await _context.Shifts.FindAsync(id);
            if (shift == null)
            {
                return NotFound();
            }
        
            _context.Shifts.Remove(shift);
            await _context.SaveChangesAsync();
        
            return NoContent();
        }

        private bool ShiftExists(int id)
        {
            return _context.Shifts.Any(e => e.Id == id);
        }
    }
}
