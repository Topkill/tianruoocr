using System;
using System.Drawing;
using ShareX.ScreenCaptureLib;

namespace TrOCR.Services.ScreenCapture
{
    public sealed class ShareXScreenCaptureService : IScreenCaptureService
    {
        public ScreenCaptureResult CaptureForOcr(ScreenCaptureRequest request)
        {
            var image = RegionCaptureTasks.GetRegionImage_Mo(
                CreateOptions(request),
                out var modeFlag,
                out var point,
                out var rectangles);

            return new ScreenCaptureResult(image, modeFlag, point, rectangles);
        }

        public ScreenCaptureResult CaptureAnnotation(Image image)
        {
            if (image == null)
                return new ScreenCaptureResult(null, string.Empty, Point.Empty, Array.Empty<Rectangle>());

            using (var canvas = new Bitmap(image))
            using (var form = new RegionCaptureForm(RegionCaptureMode.Annotation, CreateOptions(ScreenCaptureRequest.ForOcr()), canvas))
            {
                form.Image_get = false;
                form.ShowDialog();
                return new ScreenCaptureResult(form.GetResultImage(), form.Mode_flag, Point.Empty, Array.Empty<Rectangle>());
            }
        }

        private static RegionCaptureOptions CreateOptions(ScreenCaptureRequest request)
        {
            var r = request ?? ScreenCaptureRequest.ForOcr();
            return new RegionCaptureOptions
            {
                ShowMagnifier = r.ShowMagnifier,
                UseSquareMagnifier = r.UseSquareMagnifier,
                MagnifierPixelCount = r.MagnifierPixelCount,
                MagnifierPixelSize = r.MagnifierPixelSize,
                InputDelay = 0
            };
        }
    }
}
