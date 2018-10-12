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

namespace LiteCardTests
{
    [TestFixture]
    class CreateProductTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void start()
        {
            //driver = new ChromeDriver();
            //driver = new FirefoxDriver();
            InternetExplorerOptions options = new InternetExplorerOptions();
            options.RequireWindowFocus = true;
            driver = new InternetExplorerDriver(options);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void CreateOneProductTest()
        {
            //Login as admin
            driver.Url = "http://localhost/litecart/admin/";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();
            wait.Until(driver => driver.Title.ToString().Equals("My Store"));

            //Go to Catalag page
            driver.Url = driver.FindElement(By.CssSelector("div#box-apps-menu-wrapper li:nth-child(2) a")).GetAttribute("href");
            wait.Until(driver => driver.Title.ToString().Equals("Catalog | My Store"));

            //Click on *Add new product* button
            driver.FindElement(By.CssSelector("a.button[href*='edit_product']")).Click();
            wait.Until(driver => driver.Title.ToString().Equals("Add New Product | My Store"));

            //Fill in General info

            //Status
            driver.FindElement(By.CssSelector("div#tab-general input[name='status'][value='1']")).Click();
            //Name
            driver.FindElement(By.CssSelector("div#tab-general input[name='name[en]']")).SendKeys("TestProduct");
            //Code
            driver.FindElement(By.CssSelector("div#tab-general input[name='code']")).SendKeys("12345");
            //Category
            driver.FindElement(By.CssSelector("div#tab-general input[data-name='Rubber Ducks']")).Click();
            //Prodduct Group
            driver.FindElement(By.CssSelector("div#tab-general input[name*='product_groups'][value='1-3']")).Click();
            //Quantity
            driver.FindElement(By.CssSelector("div#tab-general input[name='quantity']")).Clear();
            driver.FindElement(By.CssSelector("div#tab-general input[name='quantity']")).SendKeys("7");
            //Upload file
            driver.FindElement(By.CssSelector("div#tab-general input[type='File']")).SendKeys(FilePath() + @"\blueduck.png");
            //Date Valid From
            driver.FindElement(By.CssSelector("div#tab-general input[name='date_valid_from']")).SendKeys("20.02.2018");
            //Date Valid To
            driver.FindElement(By.CssSelector("div#tab-general input[name='date_valid_to']")).SendKeys("20.02.2019");

            //Fill in Information
            //Go to Information tab
            driver.FindElement(By.CssSelector("div.tabs a[href*='information']")).Click();
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("h")));
            //wait.Until(ExpectedConditions.ElementExists(By.CssSelector("select[name='zone_code'] option")));



        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }

        private string FilePath()
        {
            var baseDir = Directory.GetParent(Directory.GetParent(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).ToString());
            return baseDir.ToString();
        }
    }
}
