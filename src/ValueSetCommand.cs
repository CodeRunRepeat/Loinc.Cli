using System;
using System.CommandLine;

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
        throw new NotImplementedException();
    }
}