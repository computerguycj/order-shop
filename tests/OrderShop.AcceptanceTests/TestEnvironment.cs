using Microsoft.AspNetCore.Mvc.Testing;

namespace OrderShop.AcceptanceTests;

// Wiring, not a layer: decides WHERE the system under test is.
// By default it starts the app in-process. Pass a baseUrl to point the same
// specs at a running copy instead, e.g. a deployed environment in CI:
//   dotnet test -- TestRunParameters.Parameter(name=\"baseUrl\", value=\"http://localhost:5080\")
[SetUpFixture]
public class TestEnvironment
{
    private static WebApplicationFactory<Program>? app;

    public static HttpClient Client { get; private set; } = null!;

    [OneTimeSetUp]
    public void Start()
    {
        var baseUrl = TestContext.Parameters.Get("baseUrl");
        if (baseUrl is not null)
        {
            Client = new HttpClient { BaseAddress = new Uri(baseUrl) };
            return;
        }

        app = new WebApplicationFactory<Program>();
        Client = app.CreateClient();
    }

    [OneTimeTearDown]
    public void Stop()
    {
        Client.Dispose();
        app?.Dispose();
    }
}
