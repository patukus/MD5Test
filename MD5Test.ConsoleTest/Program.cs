using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD5Test.ConsoleTest
{
    static class Program
    {
        static void Main(string[] args)
        {
            string input = "Ala ma kota";

            Console.WriteLine($"Mój kod: {CreateMD5ByCodeV2(input)}");
            Console.WriteLine($"Biblioteka: {CreateMD5(input)}");
        }

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public static string CreateMD5ByCodeV2(string input)
        {
            var T = CreateT();
            var R = CreateR();
            byte[] inputArray = Encoding.ASCII.GetBytes(input); //Tablica źródła
            BitArray inputBitArray = new BitArray(1);
            string inputByteString = "";
            foreach (var bytes in inputArray)
            {
                var byteString = Convert.ToString(bytes, 2);
                var byteloops = 8 - byteString.Length;
                for (int i = 0; i < byteloops; i++)
                {
                    byteString = new String(byteString.Prepend('0').ToArray());
                }
                inputByteString += byteString;
            }
            inputBitArray = ConvertFromString(inputByteString);
            var size = inputBitArray.Length;

            // Doklejenie do wiadomości wejściowej bitu o wartości 1
            inputBitArray.Length++;
            inputBitArray.Set(inputBitArray.Length-1, true);

            //Doklejenie takiej ilości zer, by ciąg składał się z 512-bitowych bloków i ostatniego niepełnego – 448-bitowego
            while(inputBitArray.Length % 512 != 448)
            {
                inputBitArray.Length++;
                inputBitArray.Set(inputBitArray.Length - 1, false);
            }

            //Doklejenie 64-bitowego (zaczynając od najmniej znaczącego bitu) licznika oznaczającego rozmiar wiadomości; w ten sposób otrzymywana wiadomość złożona jest z 512-bitowych fragmentów

            var binaryString = Convert.ToString(size, 2);
            var loops = 64 - binaryString.Length;
            for (int i = 0; i < loops; i++)
            {
                binaryString = new String(binaryString.Prepend('0').ToArray());
            }
            binaryString = new String(binaryString.ToArray());

            for (int i = 0; i < 64; i++)
            {
                inputBitArray.Length++;
                inputBitArray.Set(inputBitArray.Length - 1, binaryString[i] == '0' ? false : true);
            }
            string tmp = "";
            for (int i = 0; i < inputBitArray.Length; i++)
            {
                var t = inputBitArray[i] == true ? "1" : "0";
                tmp += t; 
            }

            //Dotąd napewno jest dobrze

            //Ustawienie stanu początkowego na 0123456789abcdeffedcba9876543210
            uint h0 = 0x01234567;
            uint h1 = 0x89abcdef;
            uint h2 = 0xfedcba98;
            uint h3 = 0x76543210;




            var L = inputBitArray.Length / 512;
            var N = 16 * L;
            BitArray[] M = new BitArray[N]; //Podział na 32 bitowe słowa
            for (int i = 0; i < N; i++)
            {
                BitArray word = new BitArray(32);
                for (int j = 0; j < word.Length; j++)
                {
                    word.Set(j, inputBitArray[i * j]);
                }
                M[i] = word;
            }

            //Uruchomienie na każdym bloku funkcji zmieniającej stan (istnieje przynajmniej jeden blok nawet dla pustego wejścia)
            var a = h0;
            var binaryStringA = Make32BitString(Convert.ToString(a, 2));
            BitArray bitArrayA = ConvertFromString(binaryStringA);
            var b = h1;
            var binaryStringB = Make32BitString(Convert.ToString(b, 2));
            BitArray bitArrayB = ConvertFromString(binaryStringB);
            var c = h2;
            var binaryStringC = Make32BitString(Convert.ToString(c, 2));
            BitArray bitArrayC = ConvertFromString(binaryStringC);
            var d = h3;
            var binaryStringD = Make32BitString(Convert.ToString(d, 2));
            BitArray bitArrayD = ConvertFromString(binaryStringD);

            BitArray f = new BitArray(1);
            int g;
            for (int q = 0; q < L; q++)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        switch (i)
                        {
                            case 0:
                                f = F(bitArrayB, bitArrayC, bitArrayD);
                                break;
                            case 1:
                                f = G(bitArrayB, bitArrayC, bitArrayD);
                                break;
                            case 2:
                                f = H(bitArrayB, bitArrayC, bitArrayD);
                                break;
                            case 3:
                                f = I(bitArrayB, bitArrayC, bitArrayD);
                                break;
                        }
                        if(i == 0)
                        {
                            g = j;
                        }
                        else
                        {
                            g = (i * j) % 16;
                        }
                        

                        BitArray temp = bitArrayD;
                        bitArrayD = bitArrayC;
                        bitArrayC = bitArrayB;

                        var result = AddModulo(bitArrayA, f, ConvertFromLong(T[g]), M[g]);

                        bitArrayB = AddModulo(bitArrayB, result.LeftShift(R[i * j]));
                        bitArrayA = temp;
                        h0 = h0 + GetuintFromBitArray(bitArrayA);
                        h1 = h1 + GetuintFromBitArray(bitArrayB);
                        h2 = h2 + GetuintFromBitArray(bitArrayC);
                        h3 = h3 + GetuintFromBitArray(bitArrayD);
                    }
                }
            }

            string hexValue = h0.ToString("X")+ h1.ToString("X")+ h2.ToString("X")+ h3.ToString("X");

            //Zwrócenie stanu po przetworzeniu ostatniego bloku jako obliczony skrót wiadomości


            return hexValue;
        }

        public static byte[] BitArrayToByteArray(BitArray bits)
        {
            byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(ret, 0);
            return ret;
        }

        public static string CreateMD5ByCode(string input)
        {

            byte[] inputArray = Encoding.ASCII.GetBytes(input); //Tablica źródła

            // Doklejenie do wiadomości wejściowej bitu o wartości 1
            Array.Resize(ref inputArray, inputArray.Length + 1);
            inputArray[inputArray.Length - 1] = 1;

            //Doklejenie takiej ilości zer, by ciąg składał się z 512-bitowych bloków i ostatniego niepełnego – 448-bitowego
            while(inputArray.Length % 512 != 448)
            {
                Array.Resize(ref inputArray, inputArray.Length + 1);
                inputArray[inputArray.Length - 1] = 0;
            }

            //Doklejenie 64-bitowego (zaczynając od najmniej znaczącego bitu) licznika oznaczającego rozmiar wiadomości; w ten sposób otrzymywana wiadomość złożona jest z 512-bitowych fragmentów
            byte[] couterArray = new byte[64];
            Int64 sumValue = inputArray.Length;
            byte[] sumValueBytes = BitConverter.GetBytes(sumValue);

            for (int i = 0; i < sumValueBytes.Length; i++)
            {
                couterArray[couterArray.Length - 1 - i] = sumValueBytes[i];
            }


            var inputArrayList = inputArray.ToList();
            var couterArrayList = couterArray.ToList();
            var finalList = new List<byte>();
            finalList.AddRange(inputArrayList);
            finalList.AddRange(couterArrayList);

            var finalInputArray = finalList.ToArray();


            //Ustawienie stanu początkowego na 0123456789abcdeffedcba9876543210
            var A = 0x01234567;

            var B = 0x89abcdef;
            var C = 0xfedcba98;
            var D = 0x76543210;



            //var L = finalInputArray.Length / 512;
            //for (int i = 0; i < L; i++)
            //{

            //}
            //Uruchomienie na każdym bloku funkcji zmieniającej stan (istnieje przynajmniej jeden blok nawet dla pustego wejścia)


            //Zwrócenie stanu po przetworzeniu ostatniego bloku jako obliczony skrót wiadomości

            return input;
        }

        public static BitArray AddModulo(BitArray a, BitArray b, long modulo = 4294967296)
        {
            long result = ((GetLongFromBitArray(a) % modulo) + (GetLongFromBitArray(b) % modulo)) % modulo;
            var binaryString = Make32BitString(Convert.ToString(result, 2));
            return ConvertFromString(binaryString);
        }

        public static BitArray AddModulo(BitArray a, BitArray b, BitArray c, BitArray d, long modulo = 4294967296)
        {
            long result = ((GetLongFromBitArray(a) % modulo) + (GetLongFromBitArray(b) % modulo) + (GetLongFromBitArray(c) % modulo) + (GetLongFromBitArray(d) % modulo)) % modulo;
            var binaryString = Make32BitString(Convert.ToString(result, 2));
            return ConvertFromString(binaryString);
        }

        public static long AddModuloUint(uint a, uint b, long modulo = 4294967296)
        {
            long result = ((a % modulo) + (b % modulo)) % modulo;
            
            return result;
        }

        public static BitArray F(BitArray x, BitArray y, BitArray z)
        {
            return x.And(y).Or(x.Not().And(z));
        }

        public static BitArray G(BitArray x, BitArray y, BitArray z)
        {
            return x.And(z).Or(y.And(z.Not()));
        }

        public static BitArray H(BitArray x, BitArray y, BitArray z)
        {
            return x.Xor(y).Xor(z);
        }

        public static BitArray I(BitArray x, BitArray y, BitArray z)
        {
            return y.Xor(x.Or(z.Not()));
        }

        public static double ToRadians(this double val)
        {
            return (Math.PI / 180) * val;
        }

        public static uint GetIntFromBitArray(BitArray bitArray)
        {

            if (bitArray.Length > 32)
                throw new ArgumentException("Argument length shall be at most 32 bits.");

            uint[] array = new uint[1];
            bitArray.CopyTo(array, 0);
            return array[0];

        }


        private static long GetLongFromBitArray(BitArray bitArray)
        {
            var array = new byte[8];
            bitArray.CopyTo(array, 0);
            return BitConverter.ToInt64(array, 0);
        }


        public static uint GetuintFromBitArray(BitArray bitArray)
        {

            if (bitArray.Length > 32)
                throw new ArgumentException("Argument length shall be at most 32 bits.");

            uint[] array = new uint[1];
            bitArray.CopyTo(array, 0);
            return array[0];

        }

        public static BitArray ConvertFromLong(long value)
        {
            var binaryString = Make32BitString(Convert.ToString(value, 2));
            BitArray bitArray = new BitArray(binaryString.Length);
            for (int i = 0; i < binaryString.Length; i++)
            {
                bitArray.Set(i, binaryString[i] == '0' ? false : true);
            }
            return bitArray;
        }

        public static BitArray ConvertFromString(string binaryString)
        {
            BitArray bitArray = new BitArray(binaryString.Length);
            for (int i = 0; i < binaryString.Length; i++)
            {
                bitArray.Set(i, binaryString[i] == '0' ? false : true);
            }
            return bitArray;
        }

        public static string Make32BitString(string binaryString)
        {
            var loops = 32 - binaryString.Length;
            for (int i = 0; i < loops; i++)
            {
                binaryString = new String(binaryString.Prepend('0').ToArray());
            }
            return binaryString;
        }

        public static long[] CreateT()
        {
            long[] T = new long[64];
            for (int i = 0; i < T.Length; i++)
            {
                T[i] = Convert.ToInt64(Math.Floor(4294967296 * Math.Abs(Math.Sin(ToRadians(i)))));
            }

            return T;
        }

        public static int[] CreateR()
        {
            int[] R = new int[] { 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 227, 12, 17, 22, 5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21 };    
            return R;
        }
    }
}
