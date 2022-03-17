using System;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using System.Linq;

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

    private async Task<Parameters> CodeSystemLookup(string code, params string[] properties)
    {
        try
        {
            string response = await httpClient.Value.CodeSystemLookup(code, properties);

            var parser = new Hl7.Fhir.Serialization.FhirJsonParser();
            return parser.Parse<Parameters>(response);
        }
        catch (FormatException fex)
        {
            throw new System.Exception(
                string.Format("Unexpected result content: {0}", fex.Message),
                fex);
        }
    }

    public async Task<string> CodeSystemDisplay(string code)
    {
        var p = await CodeSystemLookup(code, "display");
        var displayName = p.Parameter?.Find(c => c.Name == "display")?.Value;
        return displayName?.ToString() ?? "";
    }

    public async Task<string> CodeSystemVerbose(string code)
    {
        var p = await CodeSystemLookup(code);
        var serializer = new Hl7.Fhir.Serialization.FhirJsonSerializer(new Hl7.Fhir.Serialization.SerializerSettings() { Pretty = true });
        return await serializer.SerializeToStringAsync(p);
    }

    public async Task<Hl7.Fhir.Model.Coding?> CodeSystemParent(string code)
    {
        var p = await CodeSystemLookup(code, "parent");

        var parentProp = p.Parameter
            ?.Find(
                c => c.Name == "property" 
                    && c.Part != null 
                    && c.Part.Exists(cc => cc.Name == "code" && cc.Value.ToString() == "parent"))
            ?.Part
            ?.Find(cc => cc.Name == "value")
            ?.Value as Hl7.Fhir.Model.Coding;

        return parentProp;
    }

    public async Task<IEnumerable<Hl7.Fhir.Model.Coding?>> CodeSystemChildren(string code)
    {
        var p = await CodeSystemLookup(code, "child");

        var children = p.Parameter
            ?.FindAll(
                c => c.Name == "property" 
                     && c.Part != null 
                     && c.Part.Exists(cc => cc.Name == "code" && cc.Value.ToString() == "child"))
            ?.Select(
                c => c.Part?.Find(cc => cc.Name == "value") ?.Value as Hl7.Fhir.Model.Coding);

        return children ?? new Hl7.Fhir.Model.Coding?[0];
    }

    private Lazy<LoincHttpClient> httpClient = new Lazy<LoincHttpClient>();

    void IDisposable.Dispose()
    {
        this.httpClient.Value.Dispose();
    }
}
