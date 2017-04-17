namespace Gma.QrCodeNet.Encoding.Windows.Render
{
    public struct DrawingSize
    {
        /// <summary>
        /// Module pixel width
        /// </summary>
        public int ModuleSize { get; private set; }

        /// <summary>
        /// QrCode pixel width
        /// </summary>
        public int CodeWidth { get; private set; }

        public QuietZoneModules QuietZoneModules { get; private set; }

        public DrawingSize(int moduleSize, int codeWidth, QuietZoneModules quietZoneModules)
            : this()
        {
            ModuleSize = moduleSize;
            CodeWidth = codeWidth;
            this.QuietZoneModules = quietZoneModules;
        }
    }
}
