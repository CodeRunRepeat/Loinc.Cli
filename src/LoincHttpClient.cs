using System.Threading.Tasks;
using System.Net.Http;

namespace Loinc.Cli;

public class LoincHttpClient : HttpClient
{
    public LoincHttpClient()
    {
        this.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", BuildAuthenticationHeader());
    }

    public async Task<string> CodeSystemLookup(string code)
    {
        try
        {
            string url = "https://fhir.loinc.org/CodeSystem/$lookup?system=http://loinc.org&code={0}";

            var response = await this.GetAsync(string.Format(url, code));
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException hre)
        {
            throw new System.Exception(
                string.Format("Code lookup failed: {0}", hre.Message), 
                hre);
        }
    }
    private static string BuildAuthenticationHeader()
    {
        string username = System.Environment.GetEnvironmentVariable("USER_NAME") ?? "";
        string password = System.Environment.GetEnvironmentVariable("PASSWORD") ?? "";

        string headerValue = $"{username}:{password}";

        return System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(headerValue));
    }
}
