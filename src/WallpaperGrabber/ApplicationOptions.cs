namespace WallpaperGrabber;

internal class ApplicationOptions
{
    public static ApplicationOptions Default { get; } = new ApplicationOptions();

    public string OutputDirectory { get; init; } = "wallpapers";
}