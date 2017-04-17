using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;

namespace Gma.QrCodeNet.Demo
{
    public static class UsageSamples
    {
        public static void RunSample1()
        {
            Console.Write(@"Type some text to QR code: ");
            string sampleText = Console.ReadLine();
            QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.M);
            QrCode qrCode = qrEncoder.Encode(sampleText);
            for (int j = 0; j < qrCode.Matrix.Width; j++)
            {
                for (int i = 0; i < qrCode.Matrix.Width; i++)
                {

                    char charToPrint = qrCode.Matrix[i, j] ? '█' : ' ';
                    Console.Write(charToPrint);
                }
                Console.WriteLine();
            }
            Console.WriteLine(@"Press any key to quit.");
            Console.ReadKey();
        }

        public static void RunSample2()
        {
            const string helloWorld = "Hello World!";

            QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
            QrCode qrCode = qrEncoder.Encode(helloWorld);

            const int moduleSizeInPixels = 5;
            GraphicsRenderer renderer = new GraphicsRenderer(new FixedModuleSize(moduleSizeInPixels, QuietZoneModules.Two), Brushes.Black, Brushes.White);

            Panel panel = new Panel();
            Point padding = new Point(10, 16);
            DrawingSize dSize = renderer.SizeCalculator.GetSize(qrCode.Matrix.Width);
            panel.AutoSize = false;
            panel.Size = new Size(dSize.CodeWidth, dSize.CodeWidth) + new Size(2*padding.X, 2*padding.Y);

            using (Graphics graphics = panel.CreateGraphics())
            {
                renderer.Draw(graphics, qrCode.Matrix, padding);
            }

        }

        public static void RunSample3()
        {
            QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
            QrCode qrCode = qrEncoder.Encode("Hello World!");

            GraphicsRenderer renderer = new GraphicsRenderer(new FixedModuleSize(5, QuietZoneModules.Two), Brushes.Black, Brushes.White);
            using (FileStream stream = new FileStream(@"c:\temp\HelloWorld.png", FileMode.Create))
            {
                renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, stream);
            }
        }
    }
}
