using System;
using System.Drawing;

namespace TrOCR.Services.ScreenCapture
{
    public sealed class ScreenCaptureResult
    {
        public ScreenCaptureResult(Image image, string modeFlag, Point flagLocation, Rectangle[] selectedRectangles)
        {
            Image = image;
            ModeFlag = modeFlag ?? string.Empty;
            FlagLocation = flagLocation;
            SelectedRectangles = selectedRectangles ?? Array.Empty<Rectangle>();
        }

        public Image Image { get; }
        public string ModeFlag { get; }
        public Point FlagLocation { get; }
        public Rectangle[] SelectedRectangles { get; }
    }
}
