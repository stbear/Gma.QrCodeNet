using System;

namespace Gma.QrCodeNet.Encoding.Windows.Render
{
    public class FixedModuleSize : ISizeCalculation
    {
        private int m_ModuleSize;
        private int m_QuietZoneModule;

        /// <summary>
        /// Module pixel size. Have to greater than zero
        /// </summary>
        public int ModuleSize
        {
            get
            {
                return m_ModuleSize;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("ModuleSize", value, "ModuleSize can not be equal or less than zero");
                m_ModuleSize = value;
            }
        }

        /// <summary>
        /// Number of quietZone modules
        /// </summary>
        public QuietZoneModules QuietZoneModules
        {
            get
            {
                return (QuietZoneModules)m_QuietZoneModule;
            }
            set
            {
                m_QuietZoneModule = (int)value;
            }
        }

        /// <summary>
        /// FixedModuleSize is strategy for rendering QrCode with fixed module pixel size.
        /// </summary>
        /// <param name="moduleSize">Module pixel size</param>
        /// <param name="quietZoneModules">number of quiet zone modules</param>
        public FixedModuleSize(int moduleSize, QuietZoneModules quietZoneModules)
        {
            m_ModuleSize = moduleSize;
            m_QuietZoneModule = (int)quietZoneModules;
        }

        /// <summary>
        /// Interface function that use by Rendering class.
        /// </summary>
        /// <param name="matrixWidth">QrCode matrix width</param>
        /// <returns>Module pixel size and QrCode pixel width</returns>
        public DrawingSize GetSize(int matrixWidth)
        {
            int width = (m_QuietZoneModule * 2 + matrixWidth) * m_ModuleSize;
            return new DrawingSize(m_ModuleSize, width, (QuietZoneModules)m_QuietZoneModule);
        }
    }
}
