namespace ShiftLoggerCHUI.Services;
using Spectre.Console;
public class AppService
{
    private readonly ShiftApiService _shiftApiService = new ShiftApiService();
    
    internal void run()
    {
        bool exitApp = false;
        string name = AskName();
        while (!exitApp)
        { 
           string selection = OperationSelector(name);
           switch (selection)
           {
               case "Start Shift":
                   _shiftApiService.StartShift();
                   break;
               case "End Shift":
                   _shiftApiService.EndShift();
                   break;
               case "List Shifts":
                   _shiftApiService.ListShifts();
                   break;
               default:
                   exitApp = true;
                   break;
           }
           AnsiConsole.MarkupLine("You selected: [yellow]{0}[/]", selection);
        }
        AnsiConsole.MarkupLine("Thank you for logging your shifts!");
    }

    internal string AskName()
    {
        var employeeName = AnsiConsole.Ask<string>("What's your [green]name[/]?");
        return employeeName;
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
                    "exit", "Start Shift", "End Shift", "List Shifts"
                }));
        return selection;
    }
}