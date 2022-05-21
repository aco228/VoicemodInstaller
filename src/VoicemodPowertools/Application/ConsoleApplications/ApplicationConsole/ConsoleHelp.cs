using VoicemodPowertools.Services.Application.ConsoleApplications.ApplicationConsole;
using ConsoleManager = VoicemodPowertools.Infrastructure.Consoles.ConsoleManager;

namespace VoicemodPowertools.Application.ConsoleApplications.ApplicationConsole;

public class ConsoleHelp : IConsoleHelp
{
    public async Task Execute(string[] args)
    {
        foreach (var group in ConsoleManager.Current.Groups)
        {
            Console.WriteLine($"\t {group.Name.ToUpper()}");
            foreach (var command in group.Commands)
            {
                Console.WriteLine($"{command.Command, 20} \t - {command.Description}");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}