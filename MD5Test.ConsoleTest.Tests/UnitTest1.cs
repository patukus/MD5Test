using NUnit.Framework;

namespace MD5Test.ConsoleTest.Tests
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(0, 0, 0, ExpectedResult = 0)]
        [TestCase(0, 0, 1, ExpectedResult = 1)]
        [TestCase(0, 1, 0, ExpectedResult = 0)]
        [TestCase(0, 1, 1, ExpectedResult = 1)]
        [TestCase(1, 0, 0, ExpectedResult = 0)]
        [TestCase(1, 0, 1, ExpectedResult = 0)]
        [TestCase(1, 1, 0, ExpectedResult = 1)]
        [TestCase(1, 1, 1, ExpectedResult = 1)]
        public uint TestF(uint x, uint y, uint z)
        {
            var result = Program.F(x, y, z);
            return result;
        }

        [TestCase(0, 0, 0, ExpectedResult = 0)]
        [TestCase(0, 0, 1, ExpectedResult = 0)]
        [TestCase(0, 1, 0, ExpectedResult = 1)]
        [TestCase(0, 1, 1, ExpectedResult = 0)]
        [TestCase(1, 0, 0, ExpectedResult = 0)]
        [TestCase(1, 0, 1, ExpectedResult = 1)]
        [TestCase(1, 1, 0, ExpectedResult = 1)]
        [TestCase(1, 1, 1, ExpectedResult = 1)]
        public uint TestG(uint x, uint y, uint z)
        {
            var result = Program.G(x, y, z);
            return result;
        }

        [TestCase(0, 0, 0, ExpectedResult = 0)]
        [TestCase(0, 0, 1, ExpectedResult = 1)]
        [TestCase(0, 1, 0, ExpectedResult = 1)]
        [TestCase(0, 1, 1, ExpectedResult = 0)]
        [TestCase(1, 0, 0, ExpectedResult = 1)]
        [TestCase(1, 0, 1, ExpectedResult = 0)]
        [TestCase(1, 1, 0, ExpectedResult = 0)]
        [TestCase(1, 1, 1, ExpectedResult = 1)]
        public uint TestH(uint x, uint y, uint z)
        {
            var result = Program.H(x, y, z);
            return result;
        }

        [TestCase(0, 0, 0, ExpectedResult = 1)]
        [TestCase(0, 0, 1, ExpectedResult = 0)]
        [TestCase(0, 1, 0, ExpectedResult = 0)]
        [TestCase(0, 1, 1, ExpectedResult = 1)]
        [TestCase(1, 0, 0, ExpectedResult = 1)]
        [TestCase(1, 0, 1, ExpectedResult = 1)]
        [TestCase(1, 1, 0, ExpectedResult = 0)]
        [TestCase(1, 1, 1, ExpectedResult = 0)]
        public uint TestI(uint x, uint y, uint z)
        {
            var result = Program.I(x, y, z);
            return result;
        }
    }
}