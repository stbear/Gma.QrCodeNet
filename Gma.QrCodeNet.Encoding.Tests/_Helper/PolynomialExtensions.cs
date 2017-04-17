using System;

namespace Gma.QrCodeNet.Encoding.Tests
{
	public static class PolynomialExtensions
	{
		public static int[] GenerateCoeff(int length, Random randomizer)
		{
			int[] result = new int[length];
			for(int i = 0; i < length; i++)
			{
				result[i] = randomizer.Next(0, 256);
			}
			return result;
		}
		
		public static bool isEqual(int[] aArray, int[] bArray)
        {
        	int alength = aArray.Length;
        	int blength = bArray.Length;
        	if(alength != blength)
        		return false;
        	else
        	{
        		for(int i = 0; i < alength; i++)
        		{
        			if(aArray[i] != bArray[i])
        				return false;
        		}
        		return true;
        	}
        }
		
		public static sbyte[] GenerateSbyteArray(int length, Random randomizer)
		{
			sbyte[] result = new sbyte[length];
			for(int i = 0; i < length; i++)
			{
				result[i] = (sbyte)randomizer.Next(0, 256);
			}
			return result;
		}
		
		public static bool isEqual(byte[] aArray, byte[] bArray)
		{
			int alength = aArray.Length;
        	int blength = bArray.Length;
        	if(alength != blength)
        		return false;
        	else
        	{
        		for(int i = 0; i < alength; i++)
        		{
        			if(aArray[i] != bArray[i])
        				return false;
        		}
        		return true;
        	}
		}
		
		public static byte[] ToByteArray(sbyte[] input)
		{
			int length = input.Length;
			byte[] result = new byte[length];
			for(int index = 0; index < length; index++)
				result[index] = (byte)input[index];
			return result;
		}
		
		public static byte[] ConvertToByte(string[] strArray)
		{
			int count = strArray.Length;
			byte[] result = new byte[count];
			for(int index = 0; index < count; index++)
			{	
				result[index] = byte.Parse(strArray[index]);
			}
			return result;
		}
		
		public static int[] ConvertToInt(string[] strArray)
		{
			int count = strArray.Length;
			int[] result = new int[count];
			for(int index = 0; index < count; index++)
			{	
				result[index] = int.Parse(strArray[index]);
			}
			return result;
		}
		
		
	}
}
