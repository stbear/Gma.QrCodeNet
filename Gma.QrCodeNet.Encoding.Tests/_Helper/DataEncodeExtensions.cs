using System;
using System.Collections.Generic;
using com.google.zxing.qrcode.encoder;
using com.google.zxing.qrcode.decoder;
using com.google.zxing.common;
using Gma.QrCodeNet.Encoding.DataEncodation;
using Gma.QrCodeNet.Encoding.Versions;
using Gma.QrCodeNet.Encoding.DataEncodation.InputRecognition;
using GMode = Gma.QrCodeNet.Encoding.DataEncodation.Mode;
using Mode = com.google.zxing.qrcode.decoder.Mode;
using Gma.QrCodeNet.Encoding.Common;


namespace Gma.QrCodeNet.Encoding.Tests._Helper
{
	internal static class DataEncodeExtensions
	{
		/// <summary>
        /// Combine Gma.QrCodeNet.Encoding input recognition method and version control method
        /// with legacy code. To create expected answer. 
        /// This is base on assume Gma.QrCodeNet.Encoding input recognition and version control sometime
        /// give different result as legacy code. 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        internal static BitVector DataEncodeUsingReferenceImplementation(string content, ErrorCorrectionLevel ecLevel, out QRCodeInternal qrInternal)
        {
        	if(string.IsNullOrEmpty(content))
        		throw new ArgumentException("input string content can not be null or empty");
        	
        	//Choose mode
        	RecognitionStruct recognitionResult = InputRecognise.Recognise(content);
        	string encodingName = recognitionResult.EncodingName;
        	Mode mode = ConvertMode(recognitionResult.Mode);
        	
        	//append byte to databits
        	BitVector dataBits = new BitVector();
			EncoderInternal.appendBytes(content, mode, dataBits, encodingName);
			
			int dataBitsLength = dataBits.size();
			VersionControlStruct vcStruct = 
				VersionControl.InitialSetup(dataBitsLength, recognitionResult.Mode, ecLevel, recognitionResult.EncodingName);
			//ECI
			BitVector headerAndDataBits = new BitVector();
			string defaultByteMode = "iso-8859-1";
			if (mode == Mode.BYTE && !defaultByteMode.Equals(encodingName))
			{
				CharacterSetECI eci = CharacterSetECI.getCharacterSetECIByName(encodingName);
				if (eci != null)
				{
					EncoderInternal.appendECI(eci, headerAndDataBits);
				}
			}
			//Mode
			EncoderInternal.appendModeInfo(mode, headerAndDataBits);
			//Char info
			int numLetters = mode.Equals(Mode.BYTE)?dataBits.sizeInBytes():content.Length;
			EncoderInternal.appendLengthInfo(numLetters, vcStruct.VersionDetail.Version, mode, headerAndDataBits);
			//Combine with dataBits
			headerAndDataBits.appendBitVector(dataBits);
			
			// Terminate the bits properly.
			EncoderInternal.terminateBits(vcStruct.VersionDetail.NumDataBytes, headerAndDataBits);
			
			qrInternal = new QRCodeInternal();
			qrInternal.Version = vcStruct.VersionDetail.Version;
			qrInternal.MatrixWidth = vcStruct.VersionDetail.MatrixWidth;
			qrInternal.EcLevelInternal = ErrorCorrectionLevelConverter.ToInternal(ecLevel);
			qrInternal.NumTotalBytes = vcStruct.VersionDetail.NumTotalBytes;
			qrInternal.NumDataBytes = vcStruct.VersionDetail.NumDataBytes;
			qrInternal.NumRSBlocks = vcStruct.VersionDetail.NumECBlocks;
			return headerAndDataBits;
        }
        
        public static BitMatrix Encode(string content, ErrorCorrectionLevel ecLevel)
        {
			QRCodeInternal qrInternal;
			BitVector headerAndDataBits = DataEncodeUsingReferenceImplementation(content, ecLevel, out qrInternal);
			
			// Step 6: Interleave data bits with error correction code.
			BitVector finalBits = new BitVector();
			EncoderInternal.interleaveWithECBytes(headerAndDataBits, qrInternal.NumTotalBytes, qrInternal.NumDataBytes, qrInternal.NumRSBlocks, finalBits);
			
			// Step 7: Choose the mask pattern and set to "QRCodeInternal".
			ByteMatrix matrix = new ByteMatrix(qrInternal.MatrixWidth, qrInternal.MatrixWidth);
			int MaskPattern = EncoderInternal.chooseMaskPattern(finalBits, qrInternal.EcLevelInternal, qrInternal.Version, matrix);
			
			// Step 8.  Build the matrix and set it to "QRCodeInternal".
			MatrixUtil.buildMatrix(finalBits, qrInternal.EcLevelInternal, qrInternal.Version, MaskPattern, matrix);
			return matrix.ToBitMatrix();
        }
        
        public static BitVector Codeword(string content, ErrorCorrectionLevel ecLevel)
        {
			QRCodeInternal qrInternal;
			BitVector headerAndDataBits = DataEncodeUsingReferenceImplementation(content, ecLevel, out qrInternal);
			
			// Step 6: Interleave data bits with error correction code.
			BitVector finalBits = new BitVector();
			EncoderInternal.interleaveWithECBytes(headerAndDataBits, qrInternal.NumTotalBytes, qrInternal.NumDataBytes, qrInternal.NumRSBlocks, finalBits);
			
			return finalBits;
        }
        
        private static Mode ConvertMode(GMode mode)
        {
        	switch(mode)
        	{
        		case GMode.Numeric:
        			return Mode.NUMERIC;
        		case GMode.Alphanumeric:
        			return Mode.ALPHANUMERIC;
        		case GMode.EightBitByte:
        			return Mode.BYTE;
        		case GMode.Kanji:
        			return Mode.KANJI;
        		default:
        			throw new ArgumentOutOfRangeException("mode", mode, string.Format("Gma mode doesn't contain {0}", mode));
        	}
        }
	}
}
