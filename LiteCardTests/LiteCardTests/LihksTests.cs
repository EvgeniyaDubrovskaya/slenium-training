using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCardTests
{
    [TestFixture]
    class LihksTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void start()
        {
            //InternetExplorerOptions options = new InternetExplorerOptions();
            //options.RequireWindowFocus = true;
            //driver = new InternetExplorerDriver(options);
            //driver = new FirefoxDriver();
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("start-maximized");
            driver = new ChromeDriver(options);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void ClickMenu()
        {
            //Login as Admin
            driver.Url = "http://localhost/litecart/admin/";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();
            wait.Until(driver => driver.Title.ToString().Equals("My Store"));

            //Go to countries page
            driver.Url = "http://localhost/litecart/admin/?app=countries&doc=countries";
            wait.Until(driver => driver.Title.ToString().Equals("Countries | My Store"));

            //Open for edit country
            driver.FindElement(By.CssSelector("tr.row td a[title='Edit']")).Click();
            wait.Until(driver => driver.Title.ToString().Equals("Edit Country | My Store"));

            //Get all external links
            var wbLinks = driver.FindElements(By.CssSelector("form a[target='_blank']"));

            foreach(var wbLink in wbLinks)
            {
                //Get windows handlers
                string mainWindow = driver.CurrentWindowHandle;
                //Open External link
                wbLink.Click();
                //Wait for opening new window
                wait.Until(d => driver.WindowHandles.Count > 1);
                //Go to new window
                ICollection<string> handlersNew = driver.WindowHandles;
                string newWindow = "";
                foreach (string handler in handlersNew)
                {
                    if(handler != mainWindow)
                    {
                        newWindow = handler;
                        break;
                    }
                }
                driver.SwitchTo().Window(newWindow);
                driver.Close();
                driver.SwitchTo().Window(mainWindow);
            }
        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
