using System.CommandLine;

namespace Loinc.Cli;

public static class CommandLine
{
    public static async System.Threading.Tasks.Task Main(string[] args)
    {
        try
        {
            DotNetEnv.Env.Load();

            RootCommand root = new RootCommand();

            CodeSystemCommand codeCommand = new CodeSystemCommand();
            root.AddCommand(codeCommand.CreateCommand());

            await root.InvokeAsync(args);
        }
        catch (System.Exception ex)
        {
            Console.Error.WriteLine(ex.ToString());
        }
    }
}
