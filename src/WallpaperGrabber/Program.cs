using AngleSharp;
using System.Text.Json;
using ToolBox;
using WallpaperGrabber;

var uri = "https://www.bing.com/";
var configPath = Path.Combine(
    Path.GetDirectoryName(Environment.ProcessPath) ?? "", "appsettings.json");
var optioins = JsonSerializer.Deserialize<ApplicationOptions>(
    File.ReadAllText(configPath));

var config = Configuration.Default.WithDefaultLoader();
var context = BrowsingContext.New(config);
var doc = await context.OpenAsync(uri);

var name = doc.QuerySelector("#vs_cont > div.mc_caro > div > div.musCardCont > h2 > a")
    ?.TextContent
    ?? $"wallpaper_{Path.GetRandomFileName()}";
foreach (var invalidChar in Path.GetInvalidFileNameChars())
    name = name.Replace(invalidChar, '_');

var fileUri = doc.QuerySelector("#vs_cont > div.mc_caro > div > div.musCardCont > div.copyright-container > ul > li > a")
    ?.Attributes["href"].Value;

if (fileUri is not null)
{
    var client = new HttpClient();
    client.BaseAddress = new Uri(doc.Url);
    var data = await client.GetByteArrayAsync(fileUri);
    if (data is not null)
    {
        FilePathHelper.EnsureDirectory(optioins.OutputDirectory);
        await File.WriteAllBytesAsync(Path.Combine(optioins.OutputDirectory, name + Path.GetExtension(fileUri)), data);
    }
}