using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Runtime.InteropServices;

namespace Gma.QrCodeNet.Encoding.Windows.Render
{
    internal static class WriteableBitmapExtension
    {
        /// <summary>
        /// Clear all pixel with given color.
        /// </summary>
        /// <param name="wBitmap">Must not be null, or pixel width, pixel height equal to zero</param>
        /// <exception>Exception should be expect with null writeablebitmap or pixel width, height equal to zero.</exception>
        internal static void Clear(this WriteableBitmap wBitmap, Color color)
        {
            if (!(wBitmap.Format == PixelFormats.Pbgra32 || wBitmap.Format == PixelFormats.Gray8))
                return;
            byte[] col = ConvertColor(wBitmap.Format, color);

            //Currently only support two PixelFormat.
            int sizeOfColor = wBitmap.Format == PixelFormats.Pbgra32 ? 4 : 1;

            int pixelW = wBitmap.Format == PixelFormats.Gray8 ? wBitmap.BackBufferStride : wBitmap.PixelWidth;
            int pixelH = wBitmap.PixelHeight;
            int totalPixels = pixelW * pixelH;

            wBitmap.Lock();
            unsafe
            {
                byte* pixels = (byte*)wBitmap.BackBuffer;
                int length = col.Length;
                //Draw first dot color. 
                for (int index = 0; index < length; index++)
                {
                    *(pixels + index) = col[index];
                }

                int pixelIndex = 1;
                int blockPixels = 1;
                //Expand to all other pixels with Log(n) process. 
                while (pixelIndex < totalPixels)
                {
                    CopyUnmanagedMemory(pixels, 0, pixels, pixelIndex * sizeOfColor, blockPixels * sizeOfColor);

                    pixelIndex += blockPixels;
                    blockPixels = Math.Min(2 * blockPixels, totalPixels - pixelIndex);
                }
            }
            wBitmap.AddDirtyRect(new Int32Rect(0, 0, wBitmap.PixelWidth, wBitmap.PixelHeight));
            wBitmap.Unlock();

        }

        /// <summary>
        /// Draw rectangle with given color.
        /// </summary>
        /// <param name="wBitmap">Must not be null, or pixel width, pixel height equal to zero</param>
        /// <exception>Exception should be expect with null writeablebitmap or pixel width, height equal to zero.</exception>
        internal static void FillRectangle(this WriteableBitmap wBitmap, Int32Rect rectangle, Color color)
        {
            if (!(wBitmap.Format == PixelFormats.Pbgra32 || wBitmap.Format == PixelFormats.Gray8))
                return;
            byte[] col = ConvertColor(wBitmap.Format, color);

            //Currently only support two PixelFormat.
            int sizeOfColor = wBitmap.Format == PixelFormats.Pbgra32 ? 4 : 1;

            int pixelW = wBitmap.Format == PixelFormats.Gray8 ? wBitmap.BackBufferStride : wBitmap.PixelWidth;
            int pixelH = wBitmap.PixelHeight;

            if (rectangle.X >= pixelW
                || rectangle.Y >= pixelH
                || rectangle.X + rectangle.Width - 1 < 0
                || rectangle.Y + rectangle.Height - 1 < 0)
                return;

            if (rectangle.X < 0) rectangle.X = 0;
            if (rectangle.Y < 0) rectangle.Y = 0;
            if (rectangle.X + rectangle.Width >= pixelW) rectangle.Width = pixelW - rectangle.X;
            if (rectangle.Y + rectangle.Height >= pixelH) rectangle.Height = pixelH - rectangle.Y;

            wBitmap.Lock();
            unsafe
            {
                byte* pixels = (byte*)wBitmap.BackBuffer;

                int startPoint = rectangle.Y * pixelW + rectangle.X;
                int endBoundry = startPoint + rectangle.Width;
                int srcOffsetBytes = startPoint * sizeOfColor;

                int length = col.Length;
                //Draw first dot color. 
                for (int index = 0; index < length; index++)
                {
                    *(pixels + srcOffsetBytes + index) = col[index];
                }

                int pixelIndex = startPoint + 1;
                int blockPixels = 1;

                //Use first pixel color at (x, y) offset to draw first line. 
                while (pixelIndex < endBoundry)
                {
                    CopyUnmanagedMemory(pixels, srcOffsetBytes, pixels, pixelIndex * sizeOfColor, blockPixels * sizeOfColor);

                    pixelIndex += blockPixels;
                    blockPixels = Math.Min(2 * blockPixels, endBoundry - pixelIndex);
                }

                int bottomLeft = (rectangle.Y + rectangle.Height - 1) * pixelW + rectangle.X;
                //Use first line of pixel to fill up rest of rectangle. 
                for (pixelIndex = startPoint + pixelW; pixelIndex <= bottomLeft; pixelIndex += pixelW)
                {
                    CopyUnmanagedMemory(pixels, srcOffsetBytes, pixels, pixelIndex * sizeOfColor, rectangle.Width * sizeOfColor);
                }

            }
            wBitmap.AddDirtyRect(rectangle);
            wBitmap.Unlock();

        }


        private static byte[] ConvertColor(PixelFormat format, Color color)
        {
            byte[] colorByteArray;
            if (format == PixelFormats.Gray8)
            {
                colorByteArray = new byte[1];
                colorByteArray[0] = (byte)(((color.R + color.B + color.G) / 3) & 0xFF);
                return colorByteArray;
            }
            else if (format == PixelFormats.Pbgra32)
            {
                colorByteArray = new byte[4];
                int a = color.A + 1;
                colorByteArray[0] = (byte)(0xFF & ((color.B * a) >> 8));
                colorByteArray[1] = (byte)(0xFF & ((color.G * a) >> 8));
                colorByteArray[2] = (byte)(0xFF & ((color.R * a) >> 8));
                colorByteArray[3] = (byte)(0xFF & color.A);

                return colorByteArray;
            }
            else
                throw new ArgumentOutOfRangeException("Not supported PixelFormats");
        }


        private static unsafe void CopyUnmanagedMemory(byte* src, int srcOffset, byte* dst, int dstOffset, int count)
        {
            src += srcOffset;
            dst += dstOffset;

            memcpy(dst, src, count);
        }


        /// <summary>
        /// Copies characters between buffers
        /// </summary>
        /// <param name="dst">New buffer</param>
        /// <param name="src">Buffer to copy from</param>
        /// <param name="count">Number of character to copy</param>
        /// <returns></returns>
        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        private static extern unsafe byte* memcpy(
            byte* dst,
            byte* src,
            int count);
    }
}
