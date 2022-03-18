using System;
using System.CommandLine;
using System.Linq;
using Hl7.Fhir.Model;

namespace Loinc.Cli;
class ValueSetCommand : LoincCommand
{
    public enum ValueSetAction
    {
        Expand,
        Verbose,
    }
    public ValueSetCommand() : base("value-set")
    {       
    }

    public override Command CreateCommand()
    {
        var settings = new LoincCommand.OptionSettings[] 
        {
            new LoincCommand.OptionSettings() { OptionName = "--code", IsRequired = true },
            new LoincCommand.OptionSettings() { OptionName = "--action" },
        };
        return this.CreateCommand<string, ValueSetAction>(ValueSetHandler, settings);
    }

    private static async System.Threading.Tasks.Task ValueSetHandler(string code, ValueSetAction action, LoincCommandParameters parameters)
    {
        try
        {
            string? response = "";
            switch (action)
            {
                case ValueSetAction.Expand:
                    var result = await parameters.LoincClient.ValueSetExpand(code);
                    if (result != null)
                    {
                        response = result
                            .Select(r => $"{r.Code} ({r.Display})")
                            .Aggregate((l, s) => $"{l}\n{s}");
                    }
                    break;

                case ValueSetAction.Verbose:
                    var bundle = await parameters.LoincClient.ValueSetGet(code);
                    response = bundle?.ToString();
                    break;

                default:
                    throw new NotImplementedException();
            }
            
            parameters.Out.WriteLine(response);
        }
        catch (InvalidOperationException)
        {
            parameters.Error.WriteLine("Error getting value set");
        }
    }
}