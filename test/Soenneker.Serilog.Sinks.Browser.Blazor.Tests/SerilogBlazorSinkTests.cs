using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Serilog.Sinks.Browser.Blazor.Tests;

[Collection("Collection")]
public class SerilogBlazorSinkTests : FixturedUnitTest
{

    public SerilogBlazorSinkTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
    }

    [Fact]
    public void Default()
    {

    }
}
