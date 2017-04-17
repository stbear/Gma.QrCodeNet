using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;

namespace Gma.QrCodeNet.Demo
{
    public partial class MainForm : Form
    {
        private Color _lightModule = Color.FromArgb(220, 100, 203, 50);
        private Color _darkModule = Color.FromArgb(170, 20, 250, 220);

        public MainForm()
        {
            InitializeComponent();
            qrCodeImgControl1.DarkBrush = new SolidBrush(_darkModule);
            qrCodeImgControl1.LightBrush = new SolidBrush(_lightModule);
            qrCodeGraphicControl1.Text = textBoxInput.Text;
            qrCodeImgControl1.Text = textBoxInput.Text;
            ContrastCal();
        }

        private void textBoxInput_TextChanged(object sender, EventArgs e)
        {
            qrCodeGraphicControl1.Text = textBoxInput.Text;
            qrCodeImgControl1.Text = textBoxInput.Text;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
        	
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = @"PNG (*.png)|*.png|Bitmap (*.bmp)|*.bmp|Encapsuled PostScript (*.eps)|*.eps|SVG (*.svg)|*.svg";
            saveFileDialog.FileName = Path.GetFileName(GetFileNameProposal());
            saveFileDialog.DefaultExt = "png";

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

			if (saveFileDialog.FileName.EndsWith("eps"))
			{
                BitMatrix matrix = qrCodeGraphicControl1.GetQrMatrix();

                // Initialize the EPS renderer
                var renderer = new EncapsulatedPostScriptRenderer(
                    new FixedModuleSize(6, QuietZoneModules.Two), // Modules size is 6/72th inch (72 points = 1 inch)
                    new FormColor(Color.Black), new FormColor(Color.White));

                using (var file = File.Open(saveFileDialog.FileName, FileMode.CreateNew))
                {
                    renderer.WriteToStream(matrix, file);
                }
			}
            else if (saveFileDialog.FileName.EndsWith("svg"))
            {
                BitMatrix matrix = qrCodeGraphicControl1.GetQrMatrix();

                // Initialize the EPS renderer
                var renderer = new SVGRenderer(
                    new FixedModuleSize(6, QuietZoneModules.Two), // Modules size is 6/72th inch (72 points = 1 inch)
                    new FormColor(Color.FromArgb(150, 200, 200, 210)), new FormColor(Color.FromArgb(200, 255, 155, 0)));

                using (var file = File.OpenWrite(saveFileDialog.FileName))
                {
                    renderer.WriteToStream(matrix, file, false);
                }
            }
            else
            {

                //DrawingBrushRenderer dRender = new DrawingBrushRenderer(new FixedModuleSize(5, QuietZoneModules.Four));
                //BitMatrix matrix = qrCodeGraphicControl1.GetQrMatrix();
                //using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                //{
                //    dRender.WriteToStream(matrix, ImageFormatEnum.PNG, stream);
                //}

                //WriteableBitmapRenderer wRender = new WriteableBitmapRenderer(new FixedModuleSize(15, QuietZoneModules.Four));
                //BitMatrix matrix = qrCodeGraphicControl1.GetQrMatrix();
                //using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                //{
                //    wRender.WriteToStream(matrix, ImageFormatEnum.PNG, stream);
                //}

                GraphicsRenderer gRender = new GraphicsRenderer(new FixedModuleSize(30, QuietZoneModules.Four));
                BitMatrix matrix = qrCodeGraphicControl1.GetQrMatrix();
                using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    gRender.WriteToStream(matrix, ImageFormat.Png, stream, new Point(600, 600));
                }
            }
           

        }

        private string GetFileNameProposal()
        {
            return textBoxInput.Text.Length > 10 ? textBoxInput.Text.Substring(0, 10) : textBoxInput.Text;
        }

        private void checkBoxArtistic_CheckedChanged(object sender, EventArgs e)
        {
            //qrCodeControl1.Artistic = checkBoxArtistic.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = @"PNG (*.png)|*.png|Bitmap (*.bmp)|*.bmp|Encapsuled PostScript (*.eps)|*.eps|SVG (*.svg)|*.svg";
            saveFileDialog.FileName = Path.GetFileName(GetFileNameProposal());
            saveFileDialog.DefaultExt = "png";

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            QrEncoder encoder = new QrEncoder();
            QrCode qrCode;
            byte[] byteArray = new byte[] { 34, 54, 90, 200 };
            if (!encoder.TryEncode(byteArray, out qrCode))
                return;
            if (saveFileDialog.FileName.EndsWith("eps"))
            {
                BitMatrix matrix = qrCode.Matrix;

                // Initialize the EPS renderer
                var renderer = new EncapsulatedPostScriptRenderer(
                    new FixedModuleSize(6, QuietZoneModules.Two), // Modules size is 6/72th inch (72 points = 1 inch)
                    new FormColor(Color.Black), new FormColor(Color.White));

                using (var file = File.Open(saveFileDialog.FileName, FileMode.CreateNew))
                {
                    renderer.WriteToStream(matrix, file);
                }
            }
            else if (saveFileDialog.FileName.EndsWith("svg"))
            {
                BitMatrix matrix = qrCode.Matrix;

                // Initialize the EPS renderer
                var renderer = new SVGRenderer(
                    new FixedModuleSize(6, QuietZoneModules.Two), // Modules size is 6/72th inch (72 points = 1 inch)
                    new FormColor(Color.FromArgb(150, 200, 200, 210)), new FormColor(Color.FromArgb(200, 255, 155, 0)));

                using (var file = File.OpenWrite(saveFileDialog.FileName))
                {
                    renderer.WriteToStream(matrix, file, false);
                }
            }
            else
            {
                GraphicsRenderer gRender = new GraphicsRenderer(new FixedModuleSize(30, QuietZoneModules.Four));
                BitMatrix matrix = qrCode.Matrix;
                using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    gRender.WriteToStream(matrix, ImageFormat.Png, stream, new Point(600, 600));
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ColorDialog colordlg = new ColorDialog();
            SolidBrush brush = qrCodeImgControl1.LightBrush as SolidBrush;
            colordlg.Color = brush == null ? _lightModule : brush.Color;

            if (colordlg.ShowDialog() == DialogResult.OK)
            {
                qrCodeImgControl1.LightBrush = new SolidBrush(colordlg.Color);
                _lightModule = colordlg.Color;
                ContrastCal();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ColorDialog colordlg = new ColorDialog();
            SolidBrush brush = qrCodeImgControl1.DarkBrush as SolidBrush;
            colordlg.Color = brush == null ? _darkModule : brush.Color;

            if (colordlg.ShowDialog() == DialogResult.OK)
            {
                qrCodeImgControl1.DarkBrush = new SolidBrush(colordlg.Color);
                _darkModule = colordlg.Color;
                ContrastCal();
            }
        }

        private void ContrastCal()
        {
            SolidBrush darkmoduleBrush = qrCodeImgControl1.DarkBrush as SolidBrush;
            SolidBrush lightmoduleBrush = qrCodeImgControl1.LightBrush as SolidBrush;
            Color darkmodule = darkmoduleBrush == null ? _darkModule : darkmoduleBrush.Color;
            Color lightmodule = lightmoduleBrush == null ? _lightModule : lightmoduleBrush.Color;

            Contrast ctrast = ColorContrast.GetContrast(new FormColor(lightmodule), new FormColor(darkmodule));

            label1.Text = ctrast.Ratio.ToString();
        }
    }
}
