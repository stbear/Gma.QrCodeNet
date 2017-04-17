using System;
using com.google.zxing.qrcode.decoder;

namespace Gma.QrCodeNet.Encoding
{
    internal static class ErrorCorrectionLevelConverter
    {

        internal static ErrorCorrectionLevel FromInternal(ErrorCorrectionLevelInternal level)
        {
            return (ErrorCorrectionLevel) level.ordinal();
        }

        internal static ErrorCorrectionLevelInternal ToInternal(ErrorCorrectionLevel level)
        {
            switch (level)
            {
                case ErrorCorrectionLevel.L:
                    return ErrorCorrectionLevelInternal.L;
                case ErrorCorrectionLevel.M:
                    return ErrorCorrectionLevelInternal.M;
                case ErrorCorrectionLevel.Q:
                    return ErrorCorrectionLevelInternal.Q;
                case ErrorCorrectionLevel.H:
                    return ErrorCorrectionLevelInternal.H;
            }
            throw new NotSupportedException(string.Format("Error correction level {0} is not supported.", level));
        }

    }
}
