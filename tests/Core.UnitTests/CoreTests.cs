using Xunit;

namespace Core.UnitTests;

public class CoreTests
{
    [Fact]
    public void CoreTesting2()
    {
        var c = new TestedClass();
        var res = c.Test(12);

        Assert.True(res);
    }
}