// Layer 4 – the system under test.
// A deliberately tiny order API. In real life this is your application.

var app = WebApplication.Create(args);
var statuses = new Dictionary<string, string>(); // order id -> status

app.MapPost("/orders", (OrderRequest request) =>
{
    var id = Guid.NewGuid().ToString();
    statuses[id] = "Placed";
    return Results.Ok(new { id });
});

app.MapPost("/orders/{id}/dispatch", (string id) =>
{
    statuses[id] = "Dispatched";
    return Results.Ok();
});

app.MapPost("/orders/{id}/cancel", (string id) =>
{
    if (statuses[id] == "Dispatched") return Results.Conflict();   // business rule
    statuses[id] = "Cancelled";
    return Results.Ok();
});

app.MapGet("/orders/{id}", (string id) => Results.Ok(new { status = statuses[id] }));

app.Run();

public record OrderRequest(string Customer, string Address, string Product, int Quantity, string Payment);

// Lets the acceptance tests start this app in-process.
public partial class Program;
