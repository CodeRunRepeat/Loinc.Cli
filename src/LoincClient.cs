using Hl7.Fhir.Serialization;

namespace Loinc.Cli;
public class LoincClient : IDisposable
{
    public async Task<Parameters> CodeSystemLookup(string code)
    {
        try
        {
            string response = await httpClient.Value.CodeSystemLookup(code);

            var parser = new FhirJsonParser();
            return parser.Parse<Parameters>(response);
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
