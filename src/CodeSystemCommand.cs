using System;
using System.CommandLine;

namespace Loinc.Cli;
class CodeSystemCommand : LoincCommand
{
    public enum CodeSystemAction
    {
        Display,
        Verbose,
        Parent,
        Child,
    }

    public CodeSystemCommand() : base("code-system")
    {       
    }

    public override Command CreateCommand()
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

        try
        {
            switch (action)
            {
                case CodeSystemAction.Display:
                var displayName = await parameters.LoincClient.CodeSystemDisplay(code);
                parameters.Out.WriteLine(displayName);
                break;

                case CodeSystemAction.Verbose:
                    var verbose = await parameters.LoincClient.CodeSystemVerbose(code);
                    parameters.Out.WriteLine(verbose);
                    break;

                case CodeSystemAction.Parent:
                    var parentProp = await parameters.LoincClient.CodeSystemParent(code);

                    var parentCode = parentProp?.Code;
                    var parentDisplay = parentProp?.Display;

                    parameters.Out.WriteLine($"{parentCode} ({parentDisplay})");
                    break;

                case CodeSystemAction.Child:
                    var children = await parameters.LoincClient.CodeSystemChildren(code);
                    if (children == null)
                        throw new InvalidOperationException();

                    foreach (var child in children)
                    {
                        parameters.Out.WriteLine($"{child?.Code} ({child?.Display})");
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }
        }
        catch (InvalidOperationException)
        {
            parameters.Error.WriteLine("No data found");
        }
    }
}