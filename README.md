# OrderShop: the four-layer model, runnable

A worked example of Dave Farley's four-layer acceptance test model in C#/NUnit.
The code is an illustration written for this exercise, not Dave's course code.

## Layout

```
src/
  OrderShop.Domain/          business rules (Order, OrderStatus) – no HTTP, no storage
  OrderShop.Api/             layer 4, the system under test: HTTP edge + in-memory storage adapter
tests/
  acceptance/
    OrderShop.Specs/         layer 1 – test cases; can reference the DSL only
    OrderShop.Dsl/           layer 2 – domain vocabulary, defaults, chooses the driver
    OrderShop.Drivers/       layer 3 – HTTP client for the API; decides where the API runs
  unit/
    OrderShop.Domain.Tests/  unit tests for the domain rules, no HTTP
```

References only point downward: Specs → Dsl → Drivers → (over HTTP) Api → Domain.
`OrderShop.Specs` sets `DisableTransitiveProjectReferences`, so a spec that tries to use a
driver or `HttpClient` directly fails to compile.

The drivers live under `tests/`, not `src/`: they are an independent client that checks the
product from outside, and they don't ship with it.

## Run the tests

```
dotnet test
```

The acceptance specs start the API in-process. To run them against a separately running copy:

```
dotnet run --project src/OrderShop.Api --urls http://localhost:5080
ORDERSHOP_BASE_URL=http://localhost:5080 dotnet test tests/acceptance/OrderShop.Specs
```

(PowerShell: `$env:ORDERSHOP_BASE_URL = "http://localhost:5080"` before `dotnet test`.)

## Step through the layers

Open `OrderShop.slnx` in Visual Studio or Rider, put a breakpoint on `orders.CancelOrder();`
in `CancelOrderSpecs`, debug the test, and keep pressing Step Into (F11). You'll go
Spec → DSL → driver → HTTP → the `/cancel` endpoint → `Order.TryCancel()` in the domain.
