using System;
//using System.Collections.Generic;

namespace Gma.QrCodeNet.Encoding.Windows.Render
{
    public interface ISizeCalculation
    {
        DrawingSize GetSize(int matrixWidth);
    }
}
