namespace WallpaperGrabber;

using AngleSharp.Dom;
using AngleSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AngleSharp.Html.Dom;

internal class BingHomePageParserByAngleSharp : IDisposable
{
    private const string _uri = "https://www.bing.com/";

    private readonly IBrowsingContext _context;

    private readonly IDocument _document;

    public BingHomePageParserByAngleSharp()
    {
        var config = Configuration.Default.WithDefaultLoader().WithJs();
        _context = BrowsingContext.New(config);
        _document = _context.OpenAsync(_uri).Result;
    }

    public IEnumerable<WallpaperInfo> GetWallpaperList()
    {
        // #leftNav
        // #rightNav
        var item = GetCurrentWallpaper();
        yield return item;

        var leftButton = (IHtmlElement)_document.QuerySelector("#leftNav");
        leftButton.DoClick();

        item = GetCurrentWallpaper();
        yield return item;

        yield break;
    }

    public WallpaperInfo GetCurrentWallpaper()
    {
        var imageTitle = _document.QuerySelector("#vs_cont > div.mc_caro > div > div.musCardCont > h3 > a")
            ?.TextContent
            ?? string.Empty;

        var uriElement = _document.QuerySelector(
                "#vs_cont > div.mc_caro > div > div.musCardCont > div.copyright-container > ul > li > a");
        var fileUri = uriElement?.GetAttribute("href")
            ?? throw new NullReferenceException();

        return new WallpaperInfo(new Uri(new Uri(_document.DocumentUri), fileUri), imageTitle);
    }

    public void Dispose()
    {
        _document.Dispose();
        _context.Dispose();
    }
}
