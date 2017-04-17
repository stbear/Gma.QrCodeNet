/*
* Copyright 2008 ZXing authors
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/
using System;
using Gma.QrCodeNet.Encoding.Common;

namespace com.google.zxing.qrcode.encoder
{
	
	/// <author>  satorux@google.com (Satoru Takabayashi) - creator
	/// </author>
	/// <author>  dswitkin@google.com (Daniel Switkin) - ported from C++
	/// </author>
	/// <author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source 
	/// </author>
	internal sealed class MaskUtil
	{
		
		private MaskUtil()
		{
			// do nothing
		}
		
		// Apply mask penalty rule 1 and return the penalty. Find repetitive cells with the same color and
		// give penalty to them. Example: 00000 or 11111.
		internal static int applyMaskPenaltyRule1(ByteMatrix matrix)
		{
			return applyMaskPenaltyRule1Internal(matrix, true) + applyMaskPenaltyRule1Internal(matrix, false);
		}
		
		// Apply mask penalty rule 2 and return the penalty. Find 2x2 blocks with the same color and give
		// penalty to them.
		internal static int applyMaskPenaltyRule2(ByteMatrix matrix)
		{
			int penalty = 0;
		    for (int y = 0; y < matrix.Height - 1; ++y)
			{
				for (int x = 0; x < matrix.Width - 1; ++x)
				{
					int value_Renamed = matrix[x,y];
                    if (value_Renamed == matrix[x + 1, y] && value_Renamed == matrix[x, y + 1] && value_Renamed == matrix[x + 1, y + 1])
					{
						penalty += 3;
					}
				}
			}
			return penalty;
		}
		
		// Apply mask penalty rule 3 and return the penalty. Find consecutive cells of 00001011101 or
		// 10111010000, and give penalty to them.  If we find patterns like 000010111010000, we give
		// penalties twice (i.e. 40 * 2).
		internal static int applyMaskPenaltyRule3(ByteMatrix matrix)
		{
			int penalty = 0;
		    int width = matrix.Width;
			int height = matrix.Height;
			for (int y = 0; y < height; ++y)
			{
				for (int x = 0; x < width; ++x)
				{
					// Tried to simplify following conditions but failed.
					if (x + 6 < width && matrix[x,y] == 1 && matrix[x + 1,y] == 0 && matrix[x + 2,y] == 1 && matrix[x + 3,y] == 1 && matrix[x + 4,y] == 1 && matrix[x + 5,y] == 0 && matrix[x + 6,y] == 1 && ((x + 10 < width && matrix[x + 7,y] == 0 && matrix[x + 8,y] == 0 && matrix[x + 9,y] == 0 && matrix[x + 10,y] == 0) || (x - 4 >= 0 && matrix[x - 1,y] == 0 && matrix[x - 2,y] == 0 && matrix[x - 3,y] == 0 && matrix[x - 4,y] == 0)))
					{
						penalty += 40;
					}
					if (y + 6 < height && matrix[x,y] == 1 && matrix[x,y + 1] == 0 && matrix[x,y + 2] == 1 && matrix[x,y + 3] == 1 && matrix[x,y + 4] == 1 && matrix[x,y + 5] == 0 && matrix[x,y + 6] == 1 && ((y + 10 < height && matrix[x,y + 7] == 0 && matrix[x,y + 8] == 0 && matrix[x,y + 9] == 0 && matrix[x,y + 10] == 0) || (y - 4 >= 0 && matrix[x,y - 1] == 0 && matrix[x,y - 2] == 0 && matrix[x,y - 3] == 0 && matrix[x,y - 4] == 0)))
					{
						penalty += 40;
					}
				}
			}
			return penalty;
		}
		
		// Apply mask penalty rule 4 and return the penalty. Calculate the ratio of dark cells and give
		// penalty if the ratio is far from 50%. It gives 10 penalty for 5% distance. Examples:
		// -   0% => 100
		// -  40% =>  20
		// -  45% =>  10
		// -  50% =>   0
		// -  55% =>  10
		// -  55% =>  20
		// - 100% => 100
		internal static int applyMaskPenaltyRule4(ByteMatrix matrix)
		{
			int numDarkCells = 0;
		    int width = matrix.Width;
			int height = matrix.Height;
			for (int y = 0; y < height; ++y)
			{
				for (int x = 0; x < width; ++x)
				{
					if (matrix[x,y] == 1)
					{
						numDarkCells += 1;
					}
				}
			}
			int numTotalCells = matrix.Height * matrix.Width;
			double darkRatio = (double) numDarkCells / numTotalCells;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			return System.Math.Abs((int) (darkRatio * 100 - 50)) / 5 * 10;
		}
		
		// Return the mask bit for "getMaskPattern" at "x" and "y". See 8.8 of JISX0510:2004 for mask
		// pattern conditions.
		internal static bool getDataMaskBit(int maskPattern, int x, int y)
		{
			if (!QRCodeInternal.isValidMaskPattern(maskPattern))
			{
				throw new System.ArgumentException("Invalid mask pattern");
			}
			int intermediate, temp;
			switch (maskPattern)
			{
				
				case 0: 
					intermediate = (y + x) & 0x1;
					break;
				
				case 1: 
					intermediate = y & 0x1;
					break;
				
				case 2: 
					intermediate = x % 3;
					break;
				
				case 3: 
					intermediate = (y + x) % 3;
					break;
				
				case 4: 
					intermediate = ((SupportClass.URShift(y, 1)) + (x / 3)) & 0x1;
					break;
				
				case 5: 
					temp = y * x;
					intermediate = (temp & 0x1) + (temp % 3);
					break;
				
				case 6: 
					temp = y * x;
					intermediate = (((temp & 0x1) + (temp % 3)) & 0x1);
					break;
				
				case 7: 
					temp = y * x;
					intermediate = (((temp % 3) + ((y + x) & 0x1)) & 0x1);
					break;
				
				default: 
					throw new System.ArgumentException("Invalid mask pattern: " + maskPattern);
				
			}
			return intermediate == 0;
		}
		
		// Helper function for applyMaskPenaltyRule1. We need this for doing this calculation in both
		// vertical and horizontal orders respectively.
		private static int applyMaskPenaltyRule1Internal(ByteMatrix matrix, bool isHorizontal)
		{
			int penalty = 0;
			int numSameBitCells = 0;
			int prevBit = - 1;
			// Horizontal mode:
			//   for (int i = 0; i < matrix.height(); ++i) {
			//     for (int j = 0; j < matrix.width(); ++j) {
			//       int bit = matrix.get(i, j);
			// Vertical mode:
			//   for (int i = 0; i < matrix.width(); ++i) {
			//     for (int j = 0; j < matrix.height(); ++j) {
			//       int bit = matrix.get(j, i);
			int iLimit = isHorizontal?matrix.Height:matrix.Width;
			int jLimit = isHorizontal?matrix.Width:matrix.Height;
		    for (int i = 0; i < iLimit; ++i)
			{
				for (int j = 0; j < jLimit; ++j)
				{
					int bit = isHorizontal?matrix[j,i]:matrix[i,j];
					if (bit == prevBit)
					{
						numSameBitCells += 1;
						// Found five repetitive cells with the same color (bit).
						// We'll give penalty of 3.
						if (numSameBitCells == 5)
						{
							penalty += 3;
						}
						else if (numSameBitCells > 5)
						{
							// After five repetitive cells, we'll add the penalty one
							// by one.
							penalty += 1;
						}
					}
					else
					{
						numSameBitCells = 1; // Include the cell itself.
						prevBit = bit;
					}
				}
				numSameBitCells = 0; // Clear at each row/column.
			}
			return penalty;
		}
	}
}