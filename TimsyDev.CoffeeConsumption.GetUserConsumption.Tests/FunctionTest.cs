using Amazon.Lambda.TestUtilities;
using Xunit;

namespace TimsyDev.CoffeeConsumption.GetUserConsumption.Tests;

public class FunctionTest
{
    [Fact]
    public void TestToUpperFunction()
    {

        // Invoke the lambda function and confirm the string was upper cased.
        var function = new Function();
        var context = new TestLambdaContext();
        var upperCase = function.FunctionHandler("hello world", context);

        Assert.Equal("HELLO WORLD", upperCase);
    }
}
