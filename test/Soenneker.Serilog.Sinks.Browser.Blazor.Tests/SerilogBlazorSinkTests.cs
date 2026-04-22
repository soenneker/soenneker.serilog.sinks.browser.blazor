using Soenneker.Tests.HostedUnit;

namespace Soenneker.Serilog.Sinks.Browser.Blazor.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class SerilogBlazorSinkTests : HostedUnitTest
{

    public SerilogBlazorSinkTests(Host host) : base(host)
    {
    }

    [Test]
    public void Default()
    {

    }
}
