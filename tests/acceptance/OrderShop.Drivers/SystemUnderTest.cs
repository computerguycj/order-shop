using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace OrderShop.Drivers;

// Decides WHERE the system under test is, so nothing above this layer has to.
// By default it starts the app in-process. Set ORDERSHOP_BASE_URL to point the
// same specs at a running copy instead, e.g. a deployed environment in CI.
public static class SystemUnderTest
{
    private static InProcessApp? app;
    private static HttpClient? client;

    public static HttpClient Client => client ??= Connect();

    public static void Stop()
    {
        client?.Dispose();
        app?.Dispose();
        client = null;
        app = null;
    }

    private static HttpClient Connect()
    {
        var baseUrl = Environment.GetEnvironmentVariable("ORDERSHOP_BASE_URL");
        if (baseUrl is not null) return new HttpClient { BaseAddress = new Uri(baseUrl) };

        app = new InProcessApp();
        return app.CreateClient();
    }

    // The app's settings files are copied next to the test binaries, so host it from there
    // rather than relying on the app's source folder being found on disk.
    private class InProcessApp : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder) =>
            builder.UseContentRoot(AppContext.BaseDirectory);
    }
}
