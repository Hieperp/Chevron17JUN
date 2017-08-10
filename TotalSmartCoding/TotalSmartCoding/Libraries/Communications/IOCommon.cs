using System;
using System.Text;

namespace TotalSmartCoding.Libraries.Communications
{
    public class IOCommon
    {
        public static string TextToASCII(string s)
        {

            //string str = char.ConvertFromUtf32(65); //ASCII to Text

            StringBuilder stringBuilder = new StringBuilder();
            foreach (char c in s)
            {
                Console.WriteLine(System.Convert.ToInt32(c));

                stringBuilder.Append(System.Convert.ToInt32(c).ToString());

            }
            return stringBuilder.ToString();

        }


        public static string TextToHEX(string text)//string text = "abcd"
        {
            //Text To Hex

            char[] chars = text.ToCharArray();
            StringBuilder stringBuilder = new StringBuilder();
            foreach (char c in chars)
            {
                stringBuilder.Append(((Int16)c).ToString("X2"));
                stringBuilder.Append(",");
            }
            String textAsHex = stringBuilder.ToString();
            Console.WriteLine(textAsHex);
            return textAsHex;

        }

        public static string NumericTextToHEX(string text) //string text = "10"
        {
            // convert a textual representation of a number to hex

            Int32 number;
            if (Int32.TryParse(text, out number))
            {
                String textAsHex = number.ToString("X2");
                Console.WriteLine(textAsHex);

                return textAsHex;
            }
            else
                return "";

        }




        //----------------------

        public static byte[] StringToByteArrayFastest(string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");

            byte[] arr = new byte[hex.Length >> 1];

            for (int i = 0; i < hex.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }

        public static int GetHexVal(char hex)
        {
            int val = (int)hex;
            //For uppercase A-F letters:
            return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            //return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }



        //----------------------



        /// <summary>
        /// 2 byte arraries take XOR operation
        /// </summary>
        /// <param name="byStream1">first byte array</param>
        /// <param name="byStream2">second byte array</param>
        /// <returns>the result, a byte array what is the XOR result of byStrea</returns>
        public static byte[] XORBytes(byte[] byteFile1, byte[] byteFile2)
        {
            //as we know, the length of two byte arraies may be not equal,
            //in this case,we need  loop the longer one, use the zero instead of the blanks. 
            int iLength1 = byteFile1.Length;
            int iLength2 = byteFile2.Length;
            int iDeffAbs = System.Math.Abs(iLength1 - iLength2);//discrepancy 
            bool bStreamIsBigger = (iLength1 > iLength2); //true, if the first array is longer than second, otherwise false.
            int iMaxLength = bStreamIsBigger ? iLength1 : iLength2;

            byte biZero = (byte)0; // the zero will be XOR with the discrepancy

            byte[] byResult = new byte[iMaxLength]; // the result array of first array XOR with second array

            //loop......
            for (int i = 0; i < iMaxLength; i++)
            {
                //in the shorter range, both first and second array have byte
                if (i < (iMaxLength - iDeffAbs))
                {
                    byResult[i] = (byte)(byteFile1[i] ^ byteFile2[i]);
                }
                else // in the discrepancy range, one of the array have not byte, we instead of zero
                {
                    //first byte array is longer
                    if (bStreamIsBigger)
                    {
                        byResult[i] = (byte)(byteFile1[i] ^ biZero);
                    }
                    else//second byte array is longer
                    {
                        byResult[i] = (byte)(biZero ^ byteFile2[i]);
                    }
                }
            }

            return byResult;
        }


        public static byte[] CheckSumHEXString(string stringReadFrom)
        {
            string[] arrayBarcode = stringReadFrom.Split(new Char[] { (char)44 });
            byte[] result = new byte[0];
            for (int i = 0; i < arrayBarcode.Length; i++)
            {
                result = i == 0 ? IOCommon.StringToByteArrayFastest(arrayBarcode[i]) : IOCommon.XORBytes(result, IOCommon.StringToByteArrayFastest(arrayBarcode[i]));

            }

            return result;




        }





        /// <summary> Convert a string of hex digits (ex: E4 CA B2) to a byte array. </summary>
        /// <param name="s"> The string containing the hex digits (with or without spaces). </param>
        /// <returns> Returns an array of bytes. </returns>
        public static byte[] HexStringToByteArray(string s)
        {
            try
            {
                s = s.Replace(" ", "");
                byte[] buffer = new byte[s.Length / 2];
                for (int i = 0; i < s.Length; i += 2)
                    buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
                return buffer;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary> Converts an array of bytes into a formatted string of hex digits (ex: E4 CA B2)</summary>
        /// <param name="data"> The array of bytes to be translated into a string of hex digits. </param>
        /// <returns> Returns a well formatted string of hex digits with spacing. </returns>
        public static string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
            return sb.ToString().ToUpper();
        }



        public static string IntToHexString(int data) //Le Minh Hiep
        {
            return Convert.ToString(data, 16).PadLeft(2, '0').ToUpper();
        }

    }
}
