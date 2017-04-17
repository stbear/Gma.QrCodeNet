using System.IO;
using System.Text;
using Gma.QrCodeNet.Encoding.Common;

namespace Gma.QrCodeNet.Encoding.Tests
{
    public static class ByteMatrixToGraphicExtensions
    {
        private const char s_1Char = '█';
        private const char s_0Char = '░';
        private const char s_EmptyChar = '•';


        internal static string ToGraphicString(this ByteMatrix matrix)
        {
            StringBuilder result = new StringBuilder(matrix.Width * matrix.Width + matrix.Width + 5);
            using (StringWriter writer = new StringWriter(result))
            {
                matrix.ToGraphic(writer);
            }
            return result.ToString();
        }

        public static void ToGraphic(this ByteMatrix matrix, TextWriter output)
        {
            output.WriteLine(matrix.Width.ToString());
            for (int j = 0; j < matrix.Width; j++)
            {
                for (int i = 0; i < matrix.Width; i++)
                {

                    char charToPrint;
                    switch (matrix[i, j])
                    {
                        case 0:
                            charToPrint = s_0Char;
                            break;

                        case 1:
                            charToPrint = s_1Char;
                            break;

                        default:
                            charToPrint = s_EmptyChar;
                            break;

                    }
                    output.Write(charToPrint);
                }
                output.WriteLine();
            }
        }

        public static ByteMatrix FromGraphic(StreamReader input)
        {
            string widthString = input.ReadLine();
            int width = int.Parse(widthString);
            string[] lines = new string[width];

            for (int i = 0; i < width; i++)
            {
                lines[i] = input.ReadLine();
            }

            return FromGraphics(lines);
        }

        private static ByteMatrix FromGraphics(string[] lines)
        {
            var matrix = new ByteMatrix(lines.Length, lines.Length);
            for (int j = 0; j < matrix.Width; j++)
            {
                for (int i = 0; i < matrix.Width; i++)
                {
                    sbyte value = -1;
                    switch (lines[j][i])
                    {
                        case s_0Char:
                            value = 0;
                            break;
                        case s_1Char:
                            value = 1;
                            break;
                    }

                    matrix[i, j] = value;
                }
            }
            return matrix;
        }
    }
}
