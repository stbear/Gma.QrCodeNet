using System;

namespace Gma.QrCodeNet.Encoding.Windows.Render
{
    public abstract class GColor
    {
        public abstract byte R { get; }
        public abstract byte G { get; }
        public abstract byte B { get; }
        public abstract byte A { get; }
    }
}
