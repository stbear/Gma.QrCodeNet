using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests
{
    public static class BitVectorTestExtensions
    {
        public const char TrueChar = '1';
        public const char FalseChar = '0';

        public static string To01String(this IEnumerable<bool> bits)
        {
            StringBuilder result = new StringBuilder();
            foreach (bool bit in bits)
            {
                char ch = bit ? TrueChar : FalseChar;
                result.Append(ch);
            }
            return result.ToString();
        }

        internal static IEnumerable<bool> From01String(IEnumerable<char> bitsString)
        {
            foreach (char ch in bitsString)
            {
                switch (ch)
                {
                    case TrueChar:
                        yield return true;
                        break;
                    case FalseChar:
                        yield return false;
                        break;
                    default:
                        throw new ArgumentException("String is expected to contain only 0 and 1.", "bitsString");
                }
            }
        }
        
        /// <summary>
        /// Compare two IEnumerable<bool> variable with specific way.
        /// Use option "String" if you want to compare actual result manually after fail.  
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expect"></param>
        /// <param name="option">"string" Convert IEnumerable to string them compare. 
        /// "Nunit" Use nunit's own collectionAssert to compare IEnumerable.</param>
        internal static void CompareIEnumerable(IEnumerable<bool> actual, IEnumerable<bool> expect, string option)
        {
        	switch(option)
        	{
        		case "string":
        			string strResult = actual.To01String();
        			string strExpected = expect.To01String();
        	
        			if(!strResult.Equals(strExpected))
        				Assert.Fail("actual: {0} Expect: {1}", strResult, strExpected);
        			break;
        		case "Nunit":
        			CollectionAssert.AreEquivalent(expect.ToList(), actual.ToList());
        			break;
        		default:
        			throw new ArgumentException(string.Format("No such option {0}", option));
        	}
        }
    }
}
