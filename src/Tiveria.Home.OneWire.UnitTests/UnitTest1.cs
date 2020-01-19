using NUnit.Framework;

namespace Tiveria.Home.OneWire.UnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }


        // version:0, payload:29, function/ret:2, flags:263, datalen:4096, offset/owtap flags:0
        // 00 00 00 00 00 00 00 1d 00 00 00 02 00 00 01 07  |
        // 00 00 10 00 00 00 00 00 .. .. .. .. .. .. .. ..  |

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        // DirAll Message To Server
        // version:0, payload:2, function/ret:7, flags:263, datalen:0, offset/owtap flags:0
        // 00 00 00 00 00 00 00 02 00 00 00 07 00 00 01 07  |
        // 00 00 00 00 00 00 00 00 .. .. .. .. .. .. .. ..  |
    }
}