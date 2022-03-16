using System;
using System.CommandLine;
using System.Text.Json;
using System.Linq;

namespace Loinc.Cli;
class CodeSystemCommand : LoincCommand
{
    public enum CodeSystemAction
    {
        Display,
        Verbose,
        Parent,
    }

    public CodeSystemCommand() : base("code-system")
    {       
    }

    public Command CreateCommand()
    {
        var settings = new LoincCommand.OptionSettings[] 
        {
            new LoincCommand.OptionSettings() { OptionName = "--code", IsRequired = true },
            new LoincCommand.OptionSettings() { OptionName = "--action" },
        };
        return this.CreateCommand<string, CodeSystemAction>(CodeHandler, settings);
    }

    private static async System.Threading.Tasks.Task CodeHandler(string code, CodeSystemAction action, LoincCommandParameters parameters)
    {
        if (string.IsNullOrEmpty(code))
        {
            var properties = await parameters.LoincClient.CodeSystemProperties();
            parameters.Out.WriteLine(properties);
            return;
        }

        var queryProperties = 
            action == CodeSystemAction.Parent ? new string[]{ "parent" } : 
            action == CodeSystemAction.Verbose ? new string[0] : 
            new string[]{ "display" };

        try
        {
            if (action == CodeSystemAction.Display)
            {
                var displayName = await parameters.LoincClient.CodeSystemDisplay(code);
                parameters.Out.WriteLine(displayName);
            }
            else if (action == CodeSystemAction.Verbose)
            {
                var verbose = await parameters.LoincClient.CodeSystemVerbose(code);
                parameters.Out.WriteLine(verbose);
            }
            else if (action == CodeSystemAction.Parent)
            {
                var parentProp = await parameters.LoincClient.CodeSystemParent(code);

                var parentCode = parentProp?.Code;
                var parentDisplay = parentProp?.Display;

                parameters.Out.WriteLine($"{parentCode} ({parentDisplay})");
            }           
        }
        catch (InvalidOperationException)
        {
            parameters.Error.WriteLine("No data found");
        }
    }
}