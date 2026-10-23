// Layer 4 – the system under test.
// This file is only the HTTP edge: it maps requests onto the domain and
// domain results onto status codes. The business rules live in OrderShop.Domain.

using OrderShop.Api.Infrastructure;
using OrderShop.Domain;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
var app = builder.Build();

app.MapPost("/orders", (OrderRequest request, IOrderRepository orders) =>
{
    var order = new Order(Guid.NewGuid().ToString());
    orders.Add(order);
    return Results.Ok(new { id = order.Id });
});

app.MapPost("/orders/{id}/dispatch", (string id, IOrderRepository orders) =>
{
    if (orders.Find(id) is not { } order) return Results.NotFound();
    order.Dispatch();
    return Results.Ok();
});

app.MapPost("/orders/{id}/cancel", (string id, IOrderRepository orders) =>
{
    if (orders.Find(id) is not { } order) return Results.NotFound();
    return order.TryCancel() ? Results.Ok() : Results.Conflict();
});

app.MapGet("/orders/{id}", (string id, IOrderRepository orders) =>
    orders.Find(id) is { } order
        ? Results.Ok(new { status = order.Status.ToString() })
        : Results.NotFound());

app.Run();

public record OrderRequest(string Customer, string Address, string Product, int Quantity, string Payment);

// Lets the acceptance tests start this app in-process.
public partial class Program;
