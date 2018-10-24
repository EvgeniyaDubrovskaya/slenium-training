using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Reflection;

namespace LiteCardTests.app
{
    class Application
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        public Application()
        {
            driver = new ChromeDriver();
            //driver = new FirefoxDriver();
            //InternetExplorerOptions options = new InternetExplorerOptions();
            //options.RequireWindowFocus = true;
            //driver = new InternetExplorerDriver(options);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public void Quit()
        {
            driver.Quit();
            driver = null;
        }
    }
}
