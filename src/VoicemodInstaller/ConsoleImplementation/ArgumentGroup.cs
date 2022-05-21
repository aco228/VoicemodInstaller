namespace ConsoleImplementation;

public class ArgumentGroup
{
    public string Name { get; set; }
    public List<ArgumentCommand> Commands { get; set; } = new();
}