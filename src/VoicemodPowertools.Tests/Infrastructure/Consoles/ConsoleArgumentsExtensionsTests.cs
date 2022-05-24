using VoicemodPowertools.Infrastructure.Consoles;
using Xunit;

namespace VoicemodPowertools.Tests.Infrastructure.Consoles;

public class ConsoleArgumentsExtensionsTests
{
    [Theory]
    [InlineData("testFlag", true)]
    [InlineData("unknown", false)]
    [InlineData("testFlag2", true)]
    [InlineData("unkown2", false)]
    public void Should_Return_Correctly_Boolean_Flag(string flagName, bool expectedValue)
    {
        var args = new[] {"12323", "--testFlag", "other=1", "--testflag2"};
        Assert.Equal(expectedValue, args.GetValue(flagName, false));
    }

    
    [Theory]
    [InlineData("testFlag", 1)]
    [InlineData("unknown", -1)]
    [InlineData("testFlag2", 5)]
    [InlineData("unkown2", -1)]
    public void Should_Return_Correct_Int_Values(string flagName, int expectedValue)
    {
        var args = new[] {"testFlag=1", "other=1", "testflag2=5"};
        Assert.Equal(expectedValue, args.GetValue(flagName, -1));
    }

    [Fact]
    public void Should_Return_Default_String_If_There_Is_No()
    {
        var args = new[] {"testFlag=", "other=1", "testflag2=5"};
        var res = args.GetValue("testFlag", "default");
        Assert.Equal("default", res);
    }
}