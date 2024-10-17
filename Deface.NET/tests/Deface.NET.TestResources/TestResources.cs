namespace Deface.NET.TestResources;

public static class TestResources
{
    private const string MainDir = "Res";
    private const string Videos = "Videos";
    private const string Photos = "Photos";

    public const string VideosDir = $"{MainDir}/{Videos}";
    public const string PhotosDir = $"{MainDir}/{Photos}";

    public const string Video_Short_640_360_24fps = $"{VideosDir}/short_640_360_24fps.mp4";
    public const string Video_Short_HD_1280_720_24fps = $"{VideosDir}/short-hd_1280_720_24fps.mp4";
    public const string Video_Short_SD_480_270_24fps = $"{VideosDir}/short-sd_480_270_24fps.mp4";

    public const string Photo1 = $"{PhotosDir}/photo1.jpg";
    public const string Photo2 = $"{PhotosDir}/photo2.jpg";
}