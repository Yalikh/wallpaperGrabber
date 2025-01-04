namespace WallpaperGrabber;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

internal class BingHomePageParserBySelenium : IDisposable
{
    private const string _uri = "https://www.bing.com/";

    private readonly IWebDriver _driver;

    public BingHomePageParserBySelenium()
    {
        var options = new ChromeOptions();
        options.AddArgument("--headless=new");
        _driver = new ChromeDriver(options);
        _driver.Navigate().GoToUrl(_uri);
        Thread.Sleep(1000);
    }

    public IEnumerable<WallpaperInfo> GetWallpaperList()
    {
        // #leftNav
        // #rightNav
        var item = GetCurrentWallpaper();
        yield return item;

        var leftButton = _driver.FindElement(By.CssSelector("#leftNav"));
        leftButton.Click();
        Thread.Sleep(1000);

        item = GetCurrentWallpaper();
        yield return item;

        yield break;
    }

    public WallpaperInfo GetCurrentWallpaper()
    {
        var imageTitle = new WebDriverWait(
            _driver,
            TimeSpan.FromSeconds(10))
            .Until(x => x.FindElement(By.CssSelector("#vs_cont > div.mc_caro > div > div.musCardCont > h3 > a")));

        var uriElement = _driver.FindElement(
            By.CssSelector(
                "#vs_cont > div.mc_caro > div > div.musCardCont > div.copyright-container > ul > li > a"));
        var fileUri = uriElement?.GetAttribute("href")
            ?? throw new NullReferenceException();

        return new WallpaperInfo(new Uri(fileUri), imageTitle.Text);
    }

    public void Dispose() => _driver.Dispose();
}
