using System;
using System.CommandLine;
using Hl7.Fhir.Serialization;

namespace Loinc.Cli;
class CodeSystemCommand
{
    public CodeSystemCommand()
    {       
        this.LoincCommand = new LoincCommand("code-system");
    }
    private LoincCommand LoincCommand;

    public Command CreateCommand()
    {
        var settings = new LoincCommand.OptionSettings[] 
        {
            new LoincCommand.OptionSettings() { OptionName = "--code", IsRequired = true },
            new LoincCommand.OptionSettings() { OptionName = "--verbose" },
        };
        return this.LoincCommand.CreateCommand<string, bool>(CodeHandler, settings);
    }

    private static async System.Threading.Tasks.Task CodeHandler(string code, bool verbose, LoincCommandParameters parameters)
    {
        if (string.IsNullOrEmpty(code))
            throw new ArgumentException("Please provide a code.");

        var o = await parameters.LoincClient.CodeSystemLookup(code, verbose ? new string[0] : new string[]{ "display" });
        if (verbose)
        {
            var serializer = new FhirJsonSerializer(new SerializerSettings() {
                Pretty = true
            });
            parameters.Out.WriteLine(await serializer.SerializeToStringAsync(o));
        }
        else
            parameters.Out.WriteLine(o.Get("display").FirstOrDefault()?.Value);
    }
}