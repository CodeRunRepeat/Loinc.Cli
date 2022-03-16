using NUnit.Framework;
using Loinc.Cli;
using System.Threading.Tasks;

namespace Loinc.Cli.Tests;

public class LoincHttpClientTests
{
    [SetUp]
    public void Setup()
    {
        DotNetEnv.Env.Load();
        client = new LoincHttpClient();
    }
    private LoincHttpClient client;

    [Test]
    public void TestCodeSystemProperties()
    {
        var response = client.CodeSystemProperties();
        Task.WaitAll(response);
        Assert.AreNotEqual(response.Result, "");
    }

    [Test]
    public void TestCodeSystemLookup()
    {
        var response = client.CodeSystemLookup("4544-3", "display");
        Task.WaitAll(response);
        Assert.IsTrue(response.Result.IndexOf("Hematocrit [Volume Fraction] of Blood by Automated count") >= 0);
    }

    [Test]
    public void TestCodeSystemParent()
    {
        var response = client.CodeSystemLookup("LP14542-2", "parent");
        Task.WaitAll(response);
        Assert.IsTrue(response.Result.IndexOf("LP7846-1") >= 0);
    }
}