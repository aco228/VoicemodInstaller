namespace ConsoleImplementation;

public class ArgumentCommand
{
    public string Command { get; set; }
    public string Description { get; set; }
    public List<string> DescriptionMultiline { get; set; } = new();
    public Type Application { get; set; }
    public bool RequireAuth { get; set; } = false;
}