using OrderShop.Dsl;

namespace OrderShop.Specs;

[SetUpFixture]
public class SpecsSetup
{
    [OneTimeTearDown]
    public void StopSystemUnderTest() => OrdersDsl.Shutdown();
}
