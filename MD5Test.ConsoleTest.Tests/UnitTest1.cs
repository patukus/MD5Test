using NUnit.Framework;

namespace MD5Test.ConsoleTest.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(0, 0, 0, ExpectedResult = (uint)0)]
        [TestCase(0, 0, 1, ExpectedResult = (uint)1)]
        [TestCase(0, 1, 0, ExpectedResult = (uint)0)]
        [TestCase(0, 1, 1, ExpectedResult = (uint)1)]
        [TestCase(1, 0, 0, ExpectedResult = (uint)0)]
        [TestCase(1, 0, 1, ExpectedResult = (uint)0)]
        [TestCase(1, 1, 0, ExpectedResult = (uint)1)]
        [TestCase(1, 1, 1, ExpectedResult = (uint)1)]
        public uint TestF(int a, int b, int c)
        {
            uint x = (uint)a;
            uint y = (uint)b;
            uint z = (uint)c;


            var result = BinaryOperations.F(x, y, z);
            return result;
        }

        [TestCase(0, 0, 0, ExpectedResult = (uint)0)]
        [TestCase(0, 0, 1, ExpectedResult = (uint)0)]
        [TestCase(0, 1, 0, ExpectedResult = (uint)1)]
        [TestCase(0, 1, 1, ExpectedResult = (uint)0)]
        [TestCase(1, 0, 0, ExpectedResult = (uint)0)]
        [TestCase(1, 0, 1, ExpectedResult = (uint)1)]
        [TestCase(1, 1, 0, ExpectedResult = (uint)1)]
        [TestCase(1, 1, 1, ExpectedResult = (uint)1)]
        public uint TestG(int a, int b, int c)
        {
            uint x = (uint)a;
            uint y = (uint)b;
            uint z = (uint)c;

            var result = BinaryOperations.G(x, y, z);
            return result;
        }

        [TestCase(0, 0, 0, ExpectedResult = (uint)0)]
        [TestCase(0, 0, 1, ExpectedResult = (uint)1)]
        [TestCase(0, 1, 0, ExpectedResult = (uint)1)]
        [TestCase(0, 1, 1, ExpectedResult = (uint)0)]
        [TestCase(1, 0, 0, ExpectedResult = (uint)1)]
        [TestCase(1, 0, 1, ExpectedResult = (uint)0)]
        [TestCase(1, 1, 0, ExpectedResult = (uint)0)]
        [TestCase(1, 1, 1, ExpectedResult = (uint)1)]
        public uint TestH(int a, int b, int c)
        {
            uint x = (uint)a;
            uint y = (uint)b;
            uint z = (uint)c;

            var result = BinaryOperations.H(x, y, z);
            return result;
        }

        [TestCase(0, 0, 0, ExpectedResult = (uint)1)]
        [TestCase(0, 0, 1, ExpectedResult = (uint)0)]
        [TestCase(0, 1, 0, ExpectedResult = (uint)0)]
        [TestCase(0, 1, 1, ExpectedResult = (uint)1)]
        [TestCase(1, 0, 0, ExpectedResult = (uint)1)]
        [TestCase(1, 0, 1, ExpectedResult = (uint)1)]
        [TestCase(1, 1, 0, ExpectedResult = (uint)0)]
        [TestCase(1, 1, 1, ExpectedResult = (uint)0)]
        public uint TestI(int a, int b, int c)
        {
            uint x = (uint)a;
            uint y = (uint)b;
            uint z = (uint)c;

            var result = BinaryOperations.I(x, y, z);
            return result;
        }
    }
}