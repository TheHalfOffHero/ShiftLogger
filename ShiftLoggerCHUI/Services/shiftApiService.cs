using System.Net.Http.Json;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json;
using NuGet.Protocol;
using ShiftLogger.Models;

namespace ShiftLoggerCHUI.Services;

public class ShiftApiService
{
    private HttpClient _client = new HttpClient()
    {
        BaseAddress = new Uri("http://localhost:5194")
    };

    internal Shift? StartShift(string EmployeeName)
    {
        var uri = $"/api/Shift/startshift/{EmployeeName}";
        var content = new StringContent("{}", Encoding.UTF8, "application/json");
        try
        {
            var result = postResponse(uri, content);
            return result.Result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    internal Shift? EndShift(string shiftId)
    {
        var uri = $"/api/Shift/endshift/{shiftId}";
        var content = new StringContent("{}", Encoding.UTF8, "application/json");
        try
        {
            var result = putResponse(uri, content);
            return result.Result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    internal IEnumerable<Shift> ListShifts(string employeeName)
    {
        List<Shift> shifts = new List<Shift>();
        Task<String> responseBody = getResponse("/api/Shift");
        var result = responseBody.Result;
        var shiftList = JsonSerializer.Deserialize<IList<Shift>>(result);

        var desiredProperty = "EmployeeName";
        
        IEnumerable<Shift> filteredShiftsByName = shiftList.Where(p => p.GetType().GetProperty(desiredProperty) != null && p.GetType().GetProperty(desiredProperty).GetValue(p).ToString() == employeeName);
        
        return filteredShiftsByName;
    }

    internal bool areAllShiftsClosed(string name)
    {
        bool result = false;
        IEnumerable<Shift> filteredShiftsByName = ListShifts(name);
        var unendedShifts = filteredShiftsByName.Where(shift => shift.EndTime == null);

        if (!unendedShifts.Any())
        {
            result = true;
        }
        
        return result;
    }

    internal async Task<Shift?> postResponse(string uri, StringContent content ){
        var responseBody = await _client.PostAsync(uri, content);
        var result = responseBody.Content.ReadFromJsonAsync<Shift>();
        return await result;
    }

    internal async Task<Shift?> putResponse(string uri, StringContent content)
    {
        var responseBody = await _client.PutAsync(uri, content);
        var result = responseBody.Content.ReadFromJsonAsync<Shift>();
        return await result;
    }
    
    internal async Task<String> getResponse(string uri){
        var responseBody = await _client.GetStringAsync(uri);
        return responseBody;
    }
}