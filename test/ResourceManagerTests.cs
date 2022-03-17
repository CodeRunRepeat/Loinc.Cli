using NUnit.Framework;
using Loinc.Cli;
using System.Threading.Tasks;

namespace Loinc.Cli.Tests;

public class ResourceManagerTests
{
    [SetUp]
    public void Setup()
    {
        DotNetEnv.Env.Load();
        resourceManager = new ResourceManager();
    }

    private ResourceManager resourceManager;

    [Test]
    public void TestResources()
    {
        foreach (var command in CommandFactory.BuildCommandList())
        {
            TestResource(command.CommandName);
        }
    }
    void TestResource(string command, string option = "")
    {
        var description = resourceManager.GetDescription(command, option);
        Assert.IsNotNull(description);
        Assert.AreNotEqual(description, "");
    }
}