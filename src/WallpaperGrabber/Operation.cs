namespace WallpaperGrabber;

using AngleSharp;
using System;
using System.Text.Json;
using System.Threading.Tasks;

internal class Operation
{
    public async Task Perform()
    {
        var uri = "https://www.bing.com/";
        var configPath = Path.Combine(
            Path.GetDirectoryName(Environment.ProcessPath) ?? "", "appsettings.json");
        var optioins = JsonSerializer.Deserialize<ApplicationOptions>(
            File.ReadAllText(configPath))
            ?? ApplicationOptions.Default;

        var config = Configuration.Default.WithDefaultLoader();
        var context = BrowsingContext.New(config);
        var doc = await context.OpenAsync(uri);
        var link = doc.QuerySelector("#vs_cont > div.mc_caro > div > div.musCardCont > h3 > a");
        var imageTitle = link?.TextContent;
        var name = imageTitle ?? $"wallpaper_{Path.GetRandomFileName()}";
        foreach (var invalidChar in Path.GetInvalidFileNameChars())
            name = name.Replace(invalidChar, '_');

        var fileUri = doc
            .QuerySelector("#vs_cont > div.mc_caro > div > div.musCardCont > div.copyright-container > ul > li > a")
            ?.Attributes["href"]?.Value;

        if (fileUri is null)
        {
            Console.Error.WriteLine("Не удалось получить ссылку на изображение.");
            return;
        }

        var imageFullUri = new Uri(new Uri(doc.Url), fileUri);

        Console.WriteLine(imageTitle);
        Console.WriteLine($"Ссылка на изображение: {imageFullUri}");

        var client = new HttpClient();
        var data = await client.GetByteArrayAsync(imageFullUri);
        if (data is null)
        {
            Console.Error.WriteLine("Не удалось скачать изображение.");
            return;
        }

        var fileName = Path.Combine(optioins.OutputDirectory, name + Path.GetExtension(fileUri));
        await File.WriteAllBytesAsync(fileName, data);
        Console.WriteLine($"Файл сохранён: {fileName}");
    }
}