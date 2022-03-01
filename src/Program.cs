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
            Command codeCommand = new Command("code");

            var option = new Option<string>("--code");
            codeCommand.AddOption(option);
            codeCommand.SetHandler((string code) => CodeHandler(code), option);
            root.AddCommand(codeCommand);

            await root.InvokeAsync(args);
        }
        catch (System.Exception ex)
        {
            Console.Error.WriteLine(ex.ToString());
        }
    }

    private static async System.Threading.Tasks.Task CodeHandler(string code)
    {
        using (var client = new LoincClient())
        {
            try
            {
                var o = await client.CodeSystemLookup(code);
                Console.WriteLine(o.Get("display").FirstOrDefault()?.Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
