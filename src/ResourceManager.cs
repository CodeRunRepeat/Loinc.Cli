using System;

namespace Loinc.Cli;

public class ResourceManager : System.Resources.ResourceManager
{
    public ResourceManager() : base("Loinc.cli.resources.commands", System.Reflection.Assembly.GetExecutingAssembly())
    {
    }
    public string GetDescription(string command, string option = "")
    {
        var resource = GetString(command + option);
        return resource ?? "";
    }

    public static Lazy<ResourceManager> CommandsResources = new Lazy<ResourceManager>();
}
