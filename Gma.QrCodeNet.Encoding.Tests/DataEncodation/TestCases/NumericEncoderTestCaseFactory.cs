using System;
using System.Collections.Generic;
using Mode = com.google.zxing.qrcode.decoder.Mode;

namespace Gma.QrCodeNet.Encoding.Tests.DataEncodation
{
    public class NumericEncoderTestCaseFactory : EncoderTestCaseFactoryBase
    {
        protected override string CsvFileName { get { return "NumericEncoderTestDataSet.csv"; }}

        protected override string DataEncodeCsvFile { get { return "NumericDataEncodeTestDataSet.csv"; } }
        
        protected override string GenerateRandomInputString(int inputSize, Random randomizer)
        {
            return GenerateRandomInputString(inputSize, randomizer, '0', '9');
        }

        protected override IEnumerable<bool> EncodeUsingReferenceImplementation(string content)
        {
            return EncodeUsingReferenceImplementation(content, Mode.NUMERIC);
        }
    }
}
