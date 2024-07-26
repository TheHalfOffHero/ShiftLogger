using ShiftLogger.Models;

namespace ShiftLoggerCHUI.Services;
using Spectre.Console;
public class AppService
{
    private readonly ShiftApiService _shiftApiService = new ShiftApiService();
    
    internal async void run()
    {
        bool exitApp = false;
        string name = AskName();
        while (!exitApp)
        { 
           string selection = OperationSelector(name);
           switch (selection)
           {
               case "Start Shift":
                   Shift? startShift = _shiftApiService.StartShift(name);
                   PrintStartShift(startShift);
                   break;
               case "End Shift":
                   bool areAllShiftsClosed = _shiftApiService.areAllShiftsClosed(name);
                   if (areAllShiftsClosed)
                   {
                       AnsiConsole.MarkupLine("[green]All of your shifts are closed![/]");
                       break;
                   }
                   string shiftId = AskShiftId(name);
                   Shift? endShift = _shiftApiService.EndShift(shiftId);
                   PrintEndShift(endShift);
                   break;
               case "List Shifts":
                   IEnumerable<Shift> shifts = _shiftApiService.ListShifts(name);
                   PrintShifts(shifts);
                   break;
               default:
                   exitApp = true;
                   break;
           }
           AnsiConsole.MarkupLine("You selected: [yellow]{0}[/]", selection);
           AnsiConsole.MarkupLine("Press Enter to proceed");
           Console.ReadKey();
           Console.Clear();
        }
        AnsiConsole.MarkupLine("Thank you for logging your shifts!");
    }

    internal string AskName()
    {
        var employeeName = AnsiConsole.Ask<string>("What's your [green]name[/]?");
        return employeeName;
    }

    internal string? AskShiftId(string name)
    {
        string shiftId = null;
        IEnumerable<Shift> shifts = _shiftApiService.ListShifts(name);
        var filteredShiftList = shifts.Where(shift => shift.EndTime == null);
        
        AnsiConsole.MarkupLine("[red]This is a list of your currently active shifts[/]");
        IEnumerable<Shift> shiftList = filteredShiftList.ToList();
        PrintShifts(shiftList);
        
        bool validShiftId = false;
        while (!validShiftId)
        {
            shiftId = AnsiConsole.Ask<string>("Enter a valid shift Id:");
            var intShiftId = int.Parse(shiftId);
            if (shiftList.Any(shift => shift.Id == intShiftId))
            {
                validShiftId = true;
            }
            else
            {
                AnsiConsole.MarkupLine("[red]You've entered an invalid shift Id[/]");
            }
        }
        return shiftId;
    }

    internal string OperationSelector(string Name)
    {
        var selection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .EnableSearch()
                .Title($"Select Action for the Employee Names: [green]{Name}[/]?")
                .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                .AddChoices(new[]
                {
                    "Start Shift", "End Shift", "List Shifts", "exit"
                }));
        return selection;
    }

    internal void PrintEndShift(Shift? shift)
    {
        AnsiConsole.MarkupLine($"ended shift (id: [purple]{shift.Id}[/]) at [purple]{shift.EndTime}[/]");
        AnsiConsole.MarkupLine($"Total duration of the shift was [purple]{shift.Duration}[/]");
    }

    internal void PrintStartShift(Shift? shift)
    {
        if (shift != null)
        {
            AnsiConsole.MarkupLine($"started shift (id: [purple]{shift.Id}[/]) at [purple]{shift.StartTime}[/]");
        }
        else
        {
            AnsiConsole.MarkupLine("Shift failed to start, contact your administrator");
        }
    }

    internal void PrintShifts(IEnumerable<Shift> shifts)
    {
        var table = new Table();
        
        //Add relevant columns
        table.AddColumn("Id").AddColumn("Name").AddColumn("Start Time").AddColumn("End Time").AddColumn("Duration");

        foreach (var shift in shifts)
        {
            table.AddRow($"{shift.Id}", $"{shift.EmployeeName}", $"{shift.StartTime}", $"{shift.EndTime}", $"{shift.Duration}");
        }
        
        AnsiConsole.Write(table);
    }
}