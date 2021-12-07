namespace MD5Test.ConsoleTest
{
    public class BinaryOperations
    {
        public static uint F(uint x, uint y, uint z)
        {
            return (x & y) | (~x & z);
        }

        public static uint G(uint x, uint y, uint z)
        {
            return (x & z) | (y & ~z);
        }

        public static uint H(uint x, uint y, uint z)
        {
            return x ^ y ^ z;
        }

        public static uint I(uint x, uint y, uint z)
        {
            return y ^ (x | ~z);
        }
    }
}
