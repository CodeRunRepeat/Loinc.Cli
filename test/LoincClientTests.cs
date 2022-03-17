using NUnit.Framework;
using Loinc.Cli;
using System.Threading.Tasks;

namespace Loinc.Cli.Tests;

public class LoincClientTests
{
    [SetUp]
    public void Setup()
    {
        DotNetEnv.Env.Load();
        client = new LoincClient();
    }

    private LoincClient client;

    [Test]
    public void TestLogin()
    {
        string username = System.Environment.GetEnvironmentVariable(LoincHttpClient.UserNameVariable) ?? "";
        string password = System.Environment.GetEnvironmentVariable(LoincHttpClient.PasswordVariable) ?? "";

        var response = client.Login(username, password);
        Task.WaitAll(response);
        Assert.IsTrue(response.Result);
    }

    [Test]
    public void TestCodeSystemProperties()
    {
        var response = client.CodeSystemProperties();
        Task.WaitAll(response);
        Assert.IsTrue(response.Result.IndexOf("https://fhir.loinc.org/CodeSystem/loinc") >= 0);
    }

    [Test]
    public void TestCodeSystemDisplay()
    {
        var response = client.CodeSystemDisplay("4544-3");
        Task.WaitAll(response);
        Assert.AreEqual(response.Result, "Hematocrit [Volume Fraction] of Blood by Automated count");
    }

    [Test]
    public void TestCodeSystemVerbose()
    {
        var response = client.CodeSystemDisplay("4544-3");
        Task.WaitAll(response);
        Assert.IsTrue(response.Result.IndexOf("Hematocrit [Volume Fraction] of Blood by Automated count") >= 0);
    }

    [Test]
    public void TestCodeSystemParent()
    {
        var response = client.CodeSystemParent("LP14542-2");
        Task.WaitAll(response);
        Assert.IsNotNull(response.Result);
        Assert.AreEqual(response.Result?.Code, "LP7846-1");
    }
}