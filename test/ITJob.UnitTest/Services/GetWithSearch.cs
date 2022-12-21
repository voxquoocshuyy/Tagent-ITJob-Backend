namespace ITJob.UnitTest.Services;

public class GetWithSearch
{
    [Theory]
    [InlineData("0",false)]
    [InlineData("huy", true)]
    [InlineData("võ quốc huy", false)]
    [InlineData("", false)]
    [InlineData("28/11/2022", false)]
    public void TestGetWithSearch(string obj, object expected)
    {

        // bool actual = obj.GetWithSearch();
        // bool actual = true;
        //
        // Assert.Equal(actual, expected);
    }
}