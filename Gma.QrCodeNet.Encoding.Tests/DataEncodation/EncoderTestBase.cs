using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gma.QrCodeNet.Encoding.DataEncodation;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.DataEncodation
{
    public abstract class EncoderTestBase
    {
        public virtual void Test_against_reference_implementation(string inputString, IEnumerable<bool> expected)
        {
            TestOneDataRow(inputString, expected);
        }

        public virtual void Test_against_csv_DataSet(string inputString, IEnumerable<bool> expected)
        {
            TestOneDataRow(inputString, expected);
        }

        private void TestOneDataRow(string inputString, IEnumerable<bool> expected)
        {
            EncoderBase target = CreateEncoder();
            IEnumerable<bool> actualResult = target.GetDataBits(inputString);

            BitVectorTestExtensions.CompareIEnumerable(actualResult, expected, "Nunit");
        }
        
        public virtual void DataEncode_Test_against_reference_DataSet(string inputString, IEnumerable<bool> expected)
        {
        	DataEncodeTestOneDataRow(inputString, expected);
        }
        
        public virtual void DataEncode_Test_against_csv_DataSet(string inputString, IEnumerable<bool> expected)
        {
        	DataEncodeTestOneDataRow(inputString, expected);
        }
        
        private void DataEncodeTestOneDataRow(string inputString, IEnumerable<bool> expected)
        {
        	EncodationStruct eStruct = DataEncode.Encode(inputString, ErrorCorrectionLevel.H);
        	IEnumerable<bool> actualResult = eStruct.DataCodewords;
        	BitVectorTestExtensions.CompareIEnumerable(actualResult, expected, "string");
        }

        protected abstract EncoderBase CreateEncoder();
    }
}