using System.IO;
using System.Text;
using Gma.QrCodeNet.Encoding.Positioning;

namespace Gma.QrCodeNet.Encoding.Tests
{
    public static class BitMatrixToGraphicExtensions
    {
        private const char s_1Char = '█';
        private const char s_0Char = '░';


        internal static string ToGraphicString(this BitMatrix matrix)
        {
            StringBuilder result = new StringBuilder(matrix.Width * matrix.Width + matrix.Width + 5);
            using (StringWriter writer = new StringWriter(result))
            {
                matrix.ToGraphic(writer);
            }
            return result.ToString();
        }

        public static void ToGraphic(this BitMatrix matrix, TextWriter output)
        {
            output.WriteLine(matrix.Width.ToString());
            for (int j = 0; j < matrix.Width; j++)
            {
                for (int i = 0; i < matrix.Width; i++)
                {

                    char charToPrint = matrix[i, j] ? s_1Char : s_0Char;
                    output.Write(charToPrint);
                }
                output.WriteLine();
            }
        }

        public static BitMatrix FromGraphic(StreamReader input)
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
        
        public static TriStateMatrix ToTriStateMatrix(this BitMatrix input)
        {
        	int width = input.Width;
        	TriStateMatrix result = new TriStateMatrix(width);
        	for(int x = 0; x < width; x++)
        	{
        		for(int y = 0; y < width; y++)
        		{
        			result[x, y, MatrixStatus.Data] = input[x, y];
        		}
        	}
        	return result;
        }

        private static BitMatrix FromGraphics(string[] lines)
        {
            var matrix = new TriStateMatrix(lines.Length);
            for (int j = 0; j < matrix.Width; j++)
            {
                for (int i = 0; i < matrix.Width; i++)
                {
                    matrix[i, j, MatrixStatus.Data] = lines[j][i] == s_1Char;
                }
            }
            return matrix;
        }
    }
}
