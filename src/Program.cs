using System.CommandLine;
using System;

namespace Loinc.Cli;

public static class CommandLine
{
    public static async System.Threading.Tasks.Task Main(string[] args)
    {
        try
        {
            DotNetEnv.Env.Load();

            RootCommand root = new RootCommand();

            foreach (var command in CommandFactory.BuildCommandList())
                root.AddCommand(command.CreateCommand());

            await root.InvokeAsync(args);
        }
        catch (System.Exception ex)
        {
            Console.Error.WriteLine(ex.ToString());
        }
    }
}
