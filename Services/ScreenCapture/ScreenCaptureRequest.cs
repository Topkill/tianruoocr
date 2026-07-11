namespace TrOCR.Services.ScreenCapture
{
    public sealed class ScreenCaptureRequest
    {
        private ScreenCaptureRequest(bool showMagnifier, bool useSquareMagnifier, int magnifierPixelCount, int magnifierPixelSize)
        {
            ShowMagnifier = showMagnifier;
            UseSquareMagnifier = useSquareMagnifier;
            MagnifierPixelCount = magnifierPixelCount;
            MagnifierPixelSize = magnifierPixelSize;
        }

        public bool ShowMagnifier { get; }
        public bool UseSquareMagnifier { get; }
        public int MagnifierPixelCount { get; }
        public int MagnifierPixelSize { get; }

        public static ScreenCaptureRequest ForOcr()
        {
            return new ScreenCaptureRequest(false, false, 15, 10);
        }
    }
}
