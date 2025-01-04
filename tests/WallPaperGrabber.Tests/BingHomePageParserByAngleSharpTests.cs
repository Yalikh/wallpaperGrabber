namespace WallpaperGrabber.Tests;

using WallpaperGrabber;

public class BingHomePageParserByAngleSharpTests
{
    public class GetWallpaperListTests
    {
        [Test]
        public void GetsInfoAllOfImages()
        {
            // Arrange
            using var parser = new BingHomePageParserByAngleSharp();

            // Act
            var actual = parser.GetWallpaperList()
                .ToArray();

            // Assert
            actual.Should().NotBeNullOrEmpty();
            actual.Should().AllSatisfy(x => x.Should().NotBeNull());
            actual.Distinct().Count().Should().Be(actual.Length);
            actual.Should().HaveCount(7);
        }
    }

    public class GetCurrentWallpaperTests
    {
        [Test]
        public void ReturnsInfoOfCurrentImage()
        {
            // Arrange
            using var parser = new BingHomePageParserByAngleSharp();

            // Act
            var actual = parser.GetCurrentWallpaper();

            // Assert
            actual.Should().NotBeNull();
            actual.Link.IsAbsoluteUri.Should().BeTrue();
            actual.Title.Should().NotBeNullOrEmpty();
        }
    }
}