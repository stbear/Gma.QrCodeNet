using System;
using System.Windows.Media;
using System.Windows;

namespace Gma.QrCodeNet.Encoding.Windows.Render
{
    internal static class GeometryExtensions
    {
        internal static void DrawRectGeometry(this StreamGeometryContext ctx, Int32Rect rect)
        {
            if (rect.IsEmpty == true)
                return;

            ctx.BeginFigure(new Point(rect.X, rect.Y), true, true);
            ctx.LineTo(new Point(rect.X, rect.Y + rect.Height), false, false);
            ctx.LineTo(new Point(rect.X + rect.Width, rect.Y + rect.Height), false, false);
            ctx.LineTo(new Point(rect.X + rect.Width, rect.Y), false, false);
        }
    }
}
