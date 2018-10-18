using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.Events;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCardTests
{
    [TestFixture]
    class BrowserLog
    {
            private IWebDriver driver;
            private WebDriverWait wait;

            [SetUp]
            public void start()
            {
                ChromeOptions options = new ChromeOptions();
                options.AddArgument("start-maximized");
                driver = new ChromeDriver(options);
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            }

            [Test]
            public void AdminCatalogProductBrowserLogTest()
            {
                //Login as Admin
                driver.Url = "http://localhost/litecart/admin/";
                driver.FindElement(By.Name("username")).SendKeys("admin");
                driver.FindElement(By.Name("password")).SendKeys("admin");
                driver.FindElement(By.Name("login")).Click();
                wait.Until(driver => driver.Title.ToString().Equals("My Store"));

                //Go to Catalog page
                driver.Url = "http://localhost/litecart/admin/?app=catalog&doc=catalog&category_id=1";
                wait.Until(driver => driver.Title.ToString().Equals("Catalog | My Store"));

                var counter = driver.FindElements(By.CssSelector("tr.row td a[href*='product']:not([title='Edit'])")).Count;
                for (int i = 0; i < counter; i++)
                {
                    //Open product page
                    var wbProduct = driver.FindElements(By.CssSelector("tr.row td a[href*='product']:not([title='Edit'])")).ElementAt(i);
                    wbProduct.Click();
                    wait.Until(driver => driver.FindElement(By.CssSelector("h1")).GetAttribute("textContent").ToString().Contains("Edit Product:"));
                    var logs = driver.Manage().Logs.GetLog("browser");
                    if (logs.Count > 0)
                    {
                        foreach (var log in logs)
                        {
                            Console.WriteLine(log);
                        }
                    }
                    Assert.IsTrue(logs.Count == 0);
                    //Go back to catalog page
                    driver.Url = "http://localhost/litecart/admin/?app=catalog&doc=catalog&category_id=1";
                    wait.Until(driver => driver.Title.ToString().Equals("Catalog | My Store"));
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
