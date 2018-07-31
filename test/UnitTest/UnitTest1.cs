using System;
using Xunit;

namespace UnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var a = "1231231";
            Assert.Contains("123", a);
        }
    }
}
