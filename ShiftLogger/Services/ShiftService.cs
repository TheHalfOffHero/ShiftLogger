using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShiftLogger.Models;

namespace ShiftLogger.Services;

public class ShiftService
{
    private readonly ShiftLoggerContext _context;

    public ShiftService(ShiftLoggerContext context)
    {
        _context = context;
    }

    internal async Task<ActionResult<Shift>> StartShift(string employeeName, int id)
    {
        bool shiftExists = ShiftExistsCheck(id);
        Shift newShift = new Shift()
        {
            Id = id,
            EmployeeName = employeeName,
            StartTime = DateTime.Now.ToUniversalTime()
        };
        if (shiftExists)
        {
            throw new InvalidOperationException("The Shift Id already exists");
        }
        _context.Shifts.Add(newShift);
        await _context.SaveChangesAsync();
        return newShift;
    }

    internal async Task<ActionResult<Shift>> EndShift(int id)
    {
        bool shiftExists = ShiftExistsCheck(id);
        Shift shift = await _context.Shifts.FindAsync(id) ?? throw new InvalidOperationException();
        shift.EndTime = DateTime.Now.ToUniversalTime();
        _context.Entry(shift).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return shift;
    }

    internal int GetNextShiftId()
    {
        int shiftCount = _context.Shifts.Count();
        return shiftCount + 1;
    }
    private bool ShiftExistsCheck(int id)
    {
        return _context.Shifts.Any(e => e.Id == id);
    }
}