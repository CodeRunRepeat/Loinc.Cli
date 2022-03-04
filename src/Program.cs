using System.CommandLine;

namespace Loinc.Cli;

public static class CommandLine
{
    public static async System.Threading.Tasks.Task Main(string[] args)
    {
        try
        {
            DotNetEnv.Env.Load();
            (new AuthTokenManager()).LoadAuthToken();

            RootCommand root = new RootCommand();

            CodeSystemCommand codeCommand = new CodeSystemCommand();
            root.AddCommand(codeCommand.CreateCommand());

            LoginCommand loginCommand = new LoginCommand();
            root.AddCommand(loginCommand.CreateCommand());

            await root.InvokeAsync(args);
        }
        catch (System.Exception ex)
        {
            Console.Error.WriteLine(ex.ToString());
        }
    }
}
