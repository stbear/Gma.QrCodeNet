using System;
using System.Collections.Generic;
using Mode = com.google.zxing.qrcode.decoder.Mode;
using com.google.zxing.qrcode.encoder;

namespace Gma.QrCodeNet.Encoding.Tests.DataEncodation
{
    /// <summary>
    /// Description of EightBitByteEncoderTestCaseFactory.
    /// </summary>
    public class EightBitByteEncoderTestCaseFactory : EncoderTestCaseFactoryBase
    {
        protected override string CsvFileName
        {
            get
            {
                return "EightBitByteEncoderTestDataSet.csv";
            }
        }
        
        protected override string DataEncodeCsvFile { get { return "EightBitByteDataEncodeTestDataSet.csv"; } }

        protected override string GenerateRandomInputString(int inputSize, Random randomizer)
        {
            return GenerateRandomInputString(inputSize, randomizer, 'ｦ', 'ﾝ');
        }

        protected override IEnumerable<bool> EncodeUsingReferenceImplementation(string content)
        {
            // Step 2: Append "bytes" into "dataBits" in appropriate encoding.
            BitVector dataBits = new BitVector();
            EncoderInternal.appendBytes(content, Mode.BYTE, dataBits, "Shift_JIS");


            return dataBits;
        }
        
    }
}
