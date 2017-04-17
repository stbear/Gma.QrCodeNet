using System;
using System.Text;
using System.Collections.Generic;
using Mode = com.google.zxing.qrcode.decoder.Mode;

namespace Gma.QrCodeNet.Encoding.Tests.DataEncodation
{
    public class AlphanumericEncoderTestCaseFactory : EncoderTestCaseFactoryBase
    {
        protected override string CsvFileName { get { return "AlphanumericEncoderTestDataSet.csv"; } }
        
        protected override string DataEncodeCsvFile { get { return "AlphanumericDataEncodeTestDataSet.csv"; } }

        protected override string GenerateRandomInputString(int inputSize, Random randomizer)
        {
            return GenerateRandomAlphaNumInputString(inputSize, randomizer);
        }

        protected override IEnumerable<bool> EncodeUsingReferenceImplementation(string content)
        {
            return EncodeUsingReferenceImplementation(content, Mode.ALPHANUMERIC);
        }
        
        /// <summary>
        /// Table from ZXing. 
        /// </summary>
        private static readonly int[] ALPHANUMERIC_TABLE = new int[]{- 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, 36, - 1, - 1, - 1, 37, 38, - 1, - 1, - 1, - 1, 39, 40, - 1, 41, 42, 43, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 44, - 1, - 1, - 1, - 1, - 1, - 1, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, - 1, - 1, - 1, - 1, - 1};
        
        protected string GenerateRandomAlphaNumInputString(int inputSize, Random randomizer)
        {
            StringBuilder result = new StringBuilder(inputSize);
            int AlphaNumTableArraySize = ALPHANUMERIC_TABLE.Length;
            for (int i = 0; i < inputSize; i++)
            {
                
                int randomCharPos;
                do
                {
                    randomCharPos = (char)randomizer.Next(0, AlphaNumTableArraySize);
                } while (ALPHANUMERIC_TABLE[randomCharPos] < 0);

                char randomChar = (char)randomCharPos;
                result.Append(randomChar);
            }
            return result.ToString();
        }
        
    }
}
