using System.IO;
using System.Text;
using Gma.QrCodeNet.Encoding.Positioning;

namespace Gma.QrCodeNet.Encoding.Tests
{
    public static class TriStateMatrixToGraphicExtensions
    {
        private const char s_1Char = '█';
        private const char s_0Char = '░';
        private const char s_EmptyChar = '•';


        internal static string ToGraphicString(this TriStateMatrix matrix)
        {
            StringBuilder result = new StringBuilder(matrix.Width * matrix.Width + matrix.Width + 5);
            using (StringWriter writer = new StringWriter(result))
            {
                matrix.ToGraphic(writer);
            }
            return result.ToString();
        }

        public static void ToGraphic(this TriStateMatrix matrix, TextWriter output)
        {
            output.WriteLine(matrix.Width.ToString());
            for (int j = 0; j < matrix.Width; j++)
            {
                for (int i = 0; i < matrix.Width; i++)
                {
                    char charToPrint;
                    if (matrix.MStatus(i, j) == MatrixStatus.None)
                    {
                        charToPrint = s_EmptyChar;
                    }
                    else
                    {
                        charToPrint = matrix[i, j] ? s_1Char : s_0Char;
                    }
                    output.Write(charToPrint);
                }
                output.WriteLine();
            }
        }

        public static TriStateMatrix FromGraphic(StreamReader input)
        {
            string widthString = input.ReadLine();
            int width = int.Parse(widthString);
            string[] lines = new string[width];

            for (int i = 0; i < width; i++)
            {
                lines[i] = input.ReadLine();
            }

            return FromGraphic(lines);
        }

        private static TriStateMatrix FromGraphic(string[] lines)
        {
            var matrix = new TriStateMatrix(lines.Length);
            for (int j = 0; j < matrix.Width; j++)
            {
                for (int i = 0; i < matrix.Width; i++)
                {
                    if (lines[j][i]==s_0Char)
                    {
                        matrix[i, j, MatrixStatus.NoMask] = false;
                    }
                    else if (lines[j][i]==s_1Char)
                    {
                        matrix[i, j, MatrixStatus.NoMask] = true;
                    }
                }
            }
            return matrix;
        }
    }
}