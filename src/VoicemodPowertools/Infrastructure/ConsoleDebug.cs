namespace VoicemodPowertools.Infrastructure;

public static class ConsoleDebug
{
    public static void WriteLine(string input)
    {
        if (Program.OnDebug)
        {
            Console.WriteLine($"debug:: {input}");
        }
    }
}