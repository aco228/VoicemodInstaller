namespace ConsoleImplementation;

public abstract class ConsoleManagerBase
{
    private bool _hasStarted = false;
    public List<ArgumentGroup> Groups { get; private set; } = new();

    protected abstract List<ArgumentGroup> GetGroupsAndCommands();
    protected abstract Task OnRun();
    protected abstract Task OnCommand(ArgumentCommand command, string[] args);

    public void Run()
    {
        if(_hasStarted) return;
        _hasStarted = true;
        
        Groups = GetGroupsAndCommands();
        OnRun();
        CommandLoop();
    }
    
    private async Task CommandLoop()
    {
        for (;;)
        {
            var commandLine = Console.ReadLine();
            
            if (string.IsNullOrEmpty(commandLine)) continue;
            await ProcessCommand(commandLine.Trim().Split(' '));
        }
    }

    protected async Task ProcessCommand(string[] args)
    {
        if(args.Length == 0) return;
        
        var split = args.ToList();
        var command = GetCommand(split[0]);
        if (command == null)
        {
            Console.WriteLine("Unknown command. Search for '--help' for all available commands");
            return;
        }
            
        split.RemoveAt(0);
        await OnCommand(command, split.ToArray());
        
        Console.WriteLine();
    }

    private ArgumentCommand? GetCommand(string commandArgument)
    {
        commandArgument = commandArgument.ToLower();
        foreach (var group in Groups)
        {
            var command = group.Commands.FirstOrDefault(x => x.Command.ToLower().Equals(commandArgument));
            if (command != null)
                return command;
        }

        return null;
    }
}