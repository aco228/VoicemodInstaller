namespace ConsoleImplementation;

public class ArgumentGroup
{
    public string Name { get; set; }
    public bool HideFromHelp { get; set; } = false;
    public List<ArgumentCommand> Commands { get; set; } = new();
}