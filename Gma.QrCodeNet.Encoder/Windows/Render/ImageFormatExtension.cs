using System;
using System.Windows.Media.Imaging;

namespace Gma.QrCodeNet.Encoding.Windows.Render
{
    public static class ImageFormatExtension
    {
        public static BitmapEncoder ChooseEncoder(this ImageFormatEnum imageFormat)
        {
            switch (imageFormat)
            {
                case ImageFormatEnum.BMP:
                    return new BmpBitmapEncoder();
                case ImageFormatEnum.GIF:
                    return new GifBitmapEncoder();
                case ImageFormatEnum.JPEG:
                    return new JpegBitmapEncoder();
                case ImageFormatEnum.PNG:
                    return new PngBitmapEncoder();
                case ImageFormatEnum.TIFF:
                    return new TiffBitmapEncoder();
                case ImageFormatEnum.WDP:
                    return new WmpBitmapEncoder();
                default:
                    throw new ArgumentOutOfRangeException("imageFormat", imageFormat, "No such encoder support for this imageFormat");
            }
        }
    }
}
