using System;
using System.Threading.Tasks;
using Hl7.Fhir.Serialization;

namespace Loinc.Cli;
public class LoincClient : IDisposable
{
    public async Task<bool> Login(string username, string password)
    {
        try
        {
            httpClient.Value.SetAuthenticationCredentials(username, password);
            string response = await httpClient.Value.CodeSystemProperties();
            return true;
        }
        catch (UnauthorizedAccessException)
        {
            return false;
        }
    }

    public async Task<string> CodeSystemProperties()
    {
        return await httpClient.Value.CodeSystemProperties();
    }

    public async Task<Hl7.Fhir.Model.Parameters> CodeSystemLookup(string code, params string[] properties)
    {
        try
        {
            string response = await httpClient.Value.CodeSystemLookup(code, properties);

            var parser = new FhirJsonParser(new ParserSettings() { AcceptUnknownMembers = true });
            return parser.Parse<Hl7.Fhir.Model.Parameters>(response);
        }
        catch (FormatException fex)
        {
            throw new System.Exception(
                string.Format("Unexpected result content: {0}", fex.Message),
                fex);
        }
    }

    private Lazy<LoincHttpClient> httpClient = new Lazy<LoincHttpClient>();

    void IDisposable.Dispose()
    {
        this.httpClient.Value.Dispose();
    }
}
