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
    public void TestCodeSystemDisplay()
    {
        var response = client.CodeSystemDisplay("4544-3");
        Task.WaitAll(response);
        Assert.AreEqual(response.Result, "Hematocrit [Volume Fraction] of Blood by Automated count");
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