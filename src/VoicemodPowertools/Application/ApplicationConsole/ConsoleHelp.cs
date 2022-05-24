using VoicemodPowertools.Services.Application.ApplicationConsole;
using ConsoleManager = VoicemodPowertools.Infrastructure.Consoles.ConsoleManager;

namespace VoicemodPowertools.Application.ApplicationConsole;

public class ConsoleHelp : IConsoleHelp
{
    public async Task Execute(string[] args)
    {
        foreach (var group in ConsoleManager.Current.Groups)
        {
            if(group.HideFromHelp) continue;
            
            Console.WriteLine($"\t {group.Name.ToUpper()}");
            Console.WriteLine($"\t -------------------------------------------");
            foreach (var command in group.Commands)
            {
                Console.WriteLine($"{command.Command, 20} \t - {command.Description}");
                if (command.DescriptionMultiline.Count > 0)
                {
                    foreach (var extraDescription in command.DescriptionMultiline)
                        Console.WriteLine($"{"", 20} \t - {extraDescription}");
                    Console.WriteLine();
                }
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}