# OrderShop: the four-layer model, runnable

A worked example of Dave Farley's four-layer acceptance test model in C#/NUnit.
The code is an illustration written for this exercise, not Dave's course code.

| Layer | File |
|---|---|
| 1. Test cases | `tests/OrderShop.AcceptanceTests/Specs/CancelOrderSpecs.cs` |
| 2. DSL | `tests/OrderShop.AcceptanceTests/Dsl/OrdersDsl.cs` |
| 3. Protocol driver | `tests/OrderShop.AcceptanceTests/Drivers/HttpOrdersDriver.cs` (+ `IOrdersDriver.cs`) |
| 4. System under test | `src/OrderShop/Program.cs` |

`TestEnvironment.cs` is wiring, not a layer: it decides where the system under test runs.

## Run the specs

```
dotnet test
```

This starts the app in-process. To run the same specs against a separately running copy:

```
dotnet run --project src/OrderShop --urls http://localhost:5080
dotnet test -- 'TestRunParameters.Parameter(name="baseUrl", value="http://localhost:5080")'
```

## Step through the layers

Open `OrderShop.slnx` in Visual Studio or Rider, put a breakpoint on `orders.CancelOrder();`
in `CancelOrderSpecs`, debug the test, and keep pressing Step Into (F11). You'll go
Spec → DSL → driver → HTTP → the `/cancel` endpoint in `Program.cs`.
