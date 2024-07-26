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
