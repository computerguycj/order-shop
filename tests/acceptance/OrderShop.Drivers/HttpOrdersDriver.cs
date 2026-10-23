using System.Net;
using System.Net.Http.Json;

namespace OrderShop.Drivers;

// Layer 3 – the protocol driver.
// The ONLY code in the test suite that knows about URLs, HTTP verbs,
// status codes and JSON.
public class HttpOrdersDriver(HttpClient http) : IOrdersDriver
{
    public string? PlaceOrder(OrderDetails details)
    {
        var response = http.PostAsJsonAsync("/orders", details).Result;
        if (response.StatusCode != HttpStatusCode.OK) return null;
        return response.Content.ReadFromJsonAsync<IdResponse>().Result!.Id;
    }

    public bool DispatchOrder(string orderId) =>
        http.PostAsync($"/orders/{orderId}/dispatch", null).Result.StatusCode == HttpStatusCode.OK;

    public bool CancelOrder(string orderId) =>
        http.PostAsync($"/orders/{orderId}/cancel", null).Result.StatusCode == HttpStatusCode.OK;

    public string GetOrderStatus(string orderId) =>
        http.GetFromJsonAsync<StatusResponse>($"/orders/{orderId}").Result!.Status;

    private record IdResponse(string Id);
    private record StatusResponse(string Status);
}
