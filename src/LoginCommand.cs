using System.CommandLine;

namespace Loinc.Cli;

class LoginCommand : LoincCommand
{
    public LoginCommand() : base("login")
    {
    }

    public Command CreateCommand()
    {
        var settings = new LoincCommand.OptionSettings[] 
        {
            new LoincCommand.OptionSettings() { OptionName = "--username", IsRequired = true },
            new LoincCommand.OptionSettings() { OptionName = "--password", IsRequired = true },
        };
        return this.CreateCommand<string, string>(LoginHandler, settings);
    }

    private static async System.Threading.Tasks.Task LoginHandler(string username, string password, LoincCommandParameters parameters)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            throw new ArgumentException("Please provide a user name and password.");
            
        if (await parameters.LoincClient.Login(username, password))
        {
            parameters.Out.WriteLine("Credentials verified, you can use them to set the environment variables. Type loinc-cli login --help for details.");
            (new AuthTokenManager()).SaveAuthToken(username, password);
        }
        else
            parameters.Error.WriteLine("Credential validation failed");
    }
}