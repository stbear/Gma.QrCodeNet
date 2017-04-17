using System;
using System.Text;
using System.Collections.Generic;
using Mode = com.google.zxing.qrcode.decoder.Mode;

namespace Gma.QrCodeNet.Encoding.Tests.DataEncodation
{
	public class KanjiEncoderTestCaseFactory : EncoderTestCaseFactoryBase
	{
		protected override string CsvFileName { get { return "KanjiEncoderTestDataSet.csv"; } }
		
		protected override string DataEncodeCsvFile { get { return "KanjiDataEncodeTestDataSet.csv"; } }
		
		protected override string GenerateRandomInputString(int inputSize, Random randomizer)
        {
			return GenerateRandomKanjiString(inputSize, randomizer);
        }
		
		protected override IEnumerable<bool> EncodeUsingReferenceImplementation(string content)
        {
            return EncodeUsingReferenceImplementation(content, Mode.KANJI);
        }
		
		
		
		/*
         * Main part of Kanji is from two separate group.
		 *Each group separate to several table. Table is control by most significant byte. 
	     *First group: 0x81?? to 0x9F?? Second group: 0xE0?? to 0xEB??
		 *Each table's value is inside least significant byte range. 
		 *Least significant byte boundary: Lower 0x40, Upper 0xFC
		 * URL: http://interscript.sourceforge.net/interscript/doc/en_shiftjis_0003.html
		*/
		
		protected string GenerateRandomKanjiString(int inputSize, Random randomizer)
        {
        	StringBuilder result = new StringBuilder(inputSize);
        	Decoder shiftJisDecoder = System.Text.Encoding.GetEncoding("Shift_JIS").GetDecoder();
        	for(int i = 0; i < inputSize; i++)
        	{
        		
        		int RandomShiftJISChar = RandomGenerateKanjiChar(randomizer);
        		
        		byte[] bytes = ConvertShortToByte(RandomShiftJISChar);
        		int charLength = shiftJisDecoder.GetCharCount(bytes, 0, bytes.Length);
        		
        		if(charLength == 1)
        		{
        			char[] kanjiChar = new char[shiftJisDecoder.GetCharCount(bytes, 0, bytes.Length)];
        			shiftJisDecoder.GetChars(bytes, 0, bytes.Length, kanjiChar, 0);
        			result.Append(kanjiChar[0]);
        		}
        		else
        			throw new ArgumentOutOfRangeException("Random Kanji Char decode fail. Decode result contain more than one char or zero char");
        	}
        	return result.ToString();
        }

        
        private const int FIRST_LOWER_BOUNDARY = 0x889F;
        private const int FIRST_UPPER_BOUNDARY = 0x9FFC;
        
        private const int SECOND_LOWER_BOUNDARY = 0xE040;
		private const int SECOND_UPPER_BOUNDARY = 0xEAA4;
		
		
        private int RandomGenerateKanjiChar(Random randomizer)
        {
        	return randomizer.Next(0, 2) == 0 ? RandomGenerateKanjiCharFromTable(FIRST_LOWER_BOUNDARY, FIRST_UPPER_BOUNDARY, randomizer)
        		: RandomGenerateKanjiCharFromTable(SECOND_LOWER_BOUNDARY, SECOND_UPPER_BOUNDARY, randomizer);
        }
        
        private int RandomGenerateKanjiCharFromTable(int lowerBoundary, int upperBoundary, Random randomizer)
        {
        	int RandomShiftJISChar = 0;
        	do
       		{
        		RandomShiftJISChar = randomizer.Next(lowerBoundary, upperBoundary + 1);
        	} while(isCharOutsideTableRange(RandomShiftJISChar));
        	return RandomShiftJISChar;
        }
        
        private byte[] ConvertShortToByte(int value)
        {
        	byte[] converterBytes = BitConverter.GetBytes((short)value);
        	return new byte[]{converterBytes[1], converterBytes[0]};
        }
        
        
        /*
         * Shift_JIS double byte char Specification
         * ISO/IEC 18004-2006 Annex H Page 93
         */
		private const int TABLE_LOWER_BOUNDARY = 0x40;
		private const int TABLE_UPPER_BOUNDARY = 0xFC;
		private const int TABLE_EXCEPT_CHAR = 0x7F;
		
        private bool isCharOutsideTableRange(int RandomChar)
        {
        	int LeastSignificantByte = RandomChar & 0xFF;
        	return (LeastSignificantByte < TABLE_LOWER_BOUNDARY || LeastSignificantByte > TABLE_UPPER_BOUNDARY || LeastSignificantByte == TABLE_EXCEPT_CHAR);
        }
		
	}
}
