namespace ITJob.UnitTest.Services;

public class SendMessage
{
    [Theory]
    [InlineData("0",false)]
    [InlineData("huy", true)]
    [InlineData("võ quốc huy", false)]
    [InlineData("", false)]
    [InlineData("3b90ef53-8ddc-4aae-ab38-a5e7f00799d9", false)]
    public void TestSendMessage(string obj, object expected)
    {

        // bool actual = obj.GetWithSearch();
        // bool actual = true;
        //
        // Assert.Equal(actual, expected);
    }
}