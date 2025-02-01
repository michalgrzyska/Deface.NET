namespace Deface.NET.IntegrationTests.Resources;

internal static class TestData
{
    private const string Directory = "Resources/";

    public const string TextFile = Directory + "TextFile.txt";

    public static class Images
    {
        private const string ImagesDir = Directory + "Images/";

        public const string CorruptedJPG = ImagesDir + "Corrupted.jpg";
        public const string CorruptedPNG = ImagesDir + "Corrupted.png";
    }

    public static class Videos
    {
        private const string VideosDir = Directory + "Videos/";

        public const string CorruptedMP4 = VideosDir + "Corrupted.mp4";
    }
}
