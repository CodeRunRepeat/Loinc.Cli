using System;
using System.IO;

namespace Loinc.Cli;

class LoincCommandParameters
{
    public LoincCommandParameters(LoincClient loincClient, TextWriter @out, TextWriter error)
    {
        this.LoincClient = loincClient;
        this.Out = @out ?? Console.Out;
        this.Error = error ?? Console.Error;
    }
    public LoincClient LoincClient { get; set; }
    public TextWriter Out { get; set; }
    public TextWriter Error { get; set; }
}