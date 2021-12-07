using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD5Test.ConsoleTest
{
    public static class Program
    {
        static void Main(string[] args)
        {
            string input = "adfsdf";

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

            byte[] inputArray = Encoding.ASCII.GetBytes(input); 
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

            //Ustawienie stanu początkowego
            //uint h0 = 0x01234567;
            //uint h1 = 0x89abcdef;
            //uint h2 = 0xfedcba98;
            //uint h3 = 0x76543210;

            uint h0 = 0x67452301;
            uint h1 = 0xEFCDAB89;
            uint h2 = 0x98BADCFE;
            uint h3 = 0x10325476;

            


            var L = inputBitArray.Length / 512;
            var N = 16 * L;
            uint[] M = new uint[N]; //Podział na 32 bitowe słowa
            for (int i = 0; i < N; i++)
            {
                string binaryWord = "";
                for (int j = 0; j < 32; j++)
                {
                    binaryWord += inputBitArray[i+j] == true ? "1" : "0";
                }
                M[i] = Convert.ToUInt32(binaryWord,2);
            }

            //Dotąd napewno jest dobrze

            //Uruchomienie na każdym bloku funkcji zmieniającej stan (istnieje przynajmniej jeden blok nawet dla pustego wejścia)
            var a = h0;
            var b = h1;
            var c = h2;
            var d = h3;


            uint f= 0;
            int g = 0;
            for (int q = 0; q < L; q++)
            {

                for (int j = 0; j < 64; j++)
                {
                    if (j >= 0 && j <= 15)
                    {
                        f = BinaryOperations.F(b, c, d);
                        g = j;
                    }
                    else if (j >= 16 && j <= 31)
                    {
                        f = BinaryOperations.G(b, c, d);
                        g = (5*j +1) % 16;
                    }
                    else if (j >= 32 && j <= 47)
                    {
                        f = BinaryOperations.H(b, c, d);
                        g = (3 * j + 5) % 16;
                    }
                    else if (j >= 48 && j <= 63)
                    {
                        f = BinaryOperations.I(b, c, d);
                        g = (7 * j) % 16;
                    }



                    uint temp = d;
                    d = c;
                    c = b;
                    b = b + ((a + f + T[j] + M[g]) << R[j]);
                    //b = ((((a +(b & c)) | (~b & d))  + T[j] + M[g]) << R[j]);
                    a = temp;

                    h0 = a;
                    h1 = b;
                    h2 = c;
                    h3 = d;

                }

            }

            string hexValue = h0.ToString("X")+ h1.ToString("X")+ h2.ToString("X")+ h3.ToString("X");

            //Zwrócenie stanu po przetworzeniu ostatniego bloku jako obliczony skrót wiadomości


            return hexValue;
        }       

        public static uint AddModulo(uint a, uint b, long modulo = 4294967296)
        {
            long result = ((a % modulo) + (b % modulo)) % modulo;           
            return Convert.ToUInt32(result);
        }

        public static uint AddModulo(uint a, uint b, uint c, uint d, long modulo = 4294967296)
        {
            long result = ((a % modulo) + (b % modulo) + (c % modulo) + (d % modulo)) % modulo;
            return Convert.ToUInt32(result);
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
            string binaryString = "";
            for (int i = 0; i < bitArray.Length; i++)
            {
                var value = bitArray[i] == true ? "1" : "0";
                binaryString += value;
            }
            uint result = (uint)Convert.ToInt32(binaryString, 2);

            return result;

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

        public static uint[] CreateT()
        {
            uint[] T = new uint[64];
            for (uint i = 0; i < T.Length; i++)
            {
                T[i] = Convert.ToUInt32(Math.Floor(4294967296 * Math.Abs(Math.Sin(i+1))));          
            }

            return T;
        }

        public static int[] CreateR()
        {
            int[] R = new int[] { 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21 };    
            return R;
        }
    }
}
