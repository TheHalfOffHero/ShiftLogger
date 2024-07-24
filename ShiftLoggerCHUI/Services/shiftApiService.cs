using System.Text.Json;

namespace ShiftLoggerCHUI.Services;

public class ShiftApiService
{
    private HttpClient _client = new HttpClient()
    {
        BaseAddress = new Uri("http://localhost:5194")
    };
    internal void StartShift(){}

    internal void EndShift(){}

    internal async Task ListShifts()
    {
        string responseBody = await _client.GetStringAsync("/api/Shift");
        Console.WriteLine(responseBody);

    }
}