using System;
using System.IO;
using System.CommandLine;

namespace Loinc.Cli;

class LoincCommand
{
    public LoincCommand(string commandName)
    {
        this.CommandName = commandName;
        this.Out = Console.Out;
        this.Error = Console.Error;
    }

    public string CommandName { get; private set; }
    public TextWriter Out { get; set; }
    public TextWriter Error { get; set; }

    public struct OptionSettings
    {
        public string OptionName;
        public bool IsRequired;
    }

    public Command CreateCommand<T1, T2>(
        Func<T1, T2, LoincCommandParameters, System.Threading.Tasks.Task> handler,
        params OptionSettings[] optionSettings)
    {
        EnsureOptions(2, optionSettings);

        Command command = new Command(this.CommandName, ResourceManager.CommandsResources.Value.GetDescription(this.CommandName));

        var options = new Option[] {
            CreateOption<T1>(optionSettings[0], this.CommandName),
            CreateOption<T2>(optionSettings[1], this.CommandName),
        };
        foreach (var option in options)
            command.AddOption(option);

        command.SetHandler((T1 p1, T2 p2) => this.CommandHandler(p1, p2, handler), options);

        return command;
    }

    private Option<T> CreateOption<T>(OptionSettings settings, string commandName)
    {
        var o = new Option<T>(settings.OptionName, ResourceManager.CommandsResources.Value.GetDescription(commandName, settings.OptionName));
        if (settings.IsRequired)
            o.Arity = ArgumentArity.ExactlyOne;
        return o;
    }
    private void EnsureOptions(int required, params OptionSettings[] options)
    {
        if (options == null || options.Length != required)
            throw new ArgumentException($"Options provided should be {required}");
    }

    private async System.Threading.Tasks.Task CommandHandler<T1, T2>(
        T1 parameter1,
        T2 parameter2,
        Func<T1, T2, LoincCommandParameters, System.Threading.Tasks.Task> handler)
    {
        using (var client = new LoincClient())
        {
            try
            {
                LoincCommandParameters parameters = new LoincCommandParameters(
                    client,
                    this.Out,
                    this.Error);
                    
                await handler(parameter1, parameter2, parameters);
            }
            catch (UnauthorizedAccessException)
            {
                this.Error.WriteLine("Invalid or missing credentials; run the login command with a valid user name and password. Visit https://loinc.org/join/ to create your credentials.");
            }
            catch (Exception ex)
            {
                this.Error.WriteLine(ex.Message);
            }
        }
    }
}
