using System.Drawing;

namespace TrOCR.Services.ScreenCapture
{
    public interface IScreenCaptureService
    {
        ScreenCaptureResult CaptureForOcr(ScreenCaptureRequest request);

        ScreenCaptureResult CaptureAnnotation(Image image);
    }
}
