using System.Threading.Tasks;
using System.Net.Http;
using System.Linq;

namespace Loinc.Cli;

public class LoincHttpClient : HttpClient
{
    public LoincHttpClient()
    {
        this.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", BuildAuthenticationHeader());
    }

    private const string BaseUrl =  "https://fhir.loinc.org/";
    private const string CodeSystemUrl = BaseUrl + "CodeSystem/";

    public async Task<string> CodeSystemProperties()
    {
        try
        {
            string url = CodeSystemUrl + "?url=http://loinc.org";
            var response = await this.GetAsync(url);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException hre)
        {
            ExceptionHandler(hre, "Getting code system properties");
            return "";
        }
    }
    public async Task<string> CodeSystemLookup(string code, params string[] properties)
    {
        try
        {
            string url = CodeSystemUrl + "$lookup?system=http://loinc.org&code={0}";
            for (int i=0; i<properties.Length; ++i)
                url += string.Format("&&property={{{0}}}", i+1);

            properties = (new string[] { code }).Concat(properties).ToArray();

            var response = await this.GetAsync(string.Format(url, properties));
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException hre)
        {
            ExceptionHandler(hre, "Code lookup");
            return "";
        }
    }

    private void ExceptionHandler(HttpRequestException hre, string operation)
    {
        if (hre.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            throw new System.UnauthorizedAccessException();
        }

        throw new System.Exception(
            string.Format("{1}} failed: {0}", hre.Message, operation), 
            hre);
    }

    public const string UserNameVariable = "LOINC_USER_NAME";
    public const string PasswordVariable = "LOINC_PASSWORD";
    public void SetAuthenticationCredentials(string username, string password)
    {
        System.Environment.SetEnvironmentVariable(UserNameVariable, username);
        System.Environment.SetEnvironmentVariable(PasswordVariable, password);

        this.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", BuildAuthenticationHeader());
    }
    private static string BuildAuthenticationHeader()
    {
        string username = System.Environment.GetEnvironmentVariable(UserNameVariable) ?? "";
        string password = System.Environment.GetEnvironmentVariable(PasswordVariable) ?? "";

        string headerValue = $"{username}:{password}";

        return System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(headerValue));
    }
}
