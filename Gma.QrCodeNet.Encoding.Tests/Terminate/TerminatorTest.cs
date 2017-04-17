using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using NUnit.Framework;
using Gma.QrCodeNet.Encoding.Terminate;

namespace Gma.QrCodeNet.Encoding.Tests.Terminate
{
	[TestFixture]
	public class TerminatorTest
	{
		[Test]
        [TestCaseSource(typeof(TerminatorTestCaseFactory), "TestCasesFromReferenceImplementation")]
        public void Test_against_reference_implementation(IEnumerable<bool> data, int numTotalByte, IEnumerable<bool> expected)
        {
        	TestOneData(data, numTotalByte, expected);
		}
        
        [Test]
        [TestCaseSource(typeof(TerminatorTestCaseFactory), "TestCasesFromCsvFile")]
        public void Test_against_TXT_Dataset(IEnumerable<bool> data, int numTotalByte, IEnumerable<bool> expected)
        {
        	TestOneData(data, numTotalByte, expected);
        }
        
        private void TestOneData(IEnumerable<bool> data, int numTotalByte, IEnumerable<bool> expected)
        {
        	BitList tData = new BitList();
        	tData.Add(data);
        	tData.TerminateBites(tData.Count, numTotalByte);
        	
        	IEnumerable actualResult = tData;
        	
        	CollectionAssert.AreEquivalent(expected.ToList(), actualResult);
        }
        
        //[Test]
        public void Generate()
        {
        	new TerminatorTestCaseFactory().GenerateTestDataSet();
        }
        
        
	}
}
