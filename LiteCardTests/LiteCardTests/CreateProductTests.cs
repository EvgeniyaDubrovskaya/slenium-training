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
            driver = new ChromeDriver();
            //driver = new FirefoxDriver();
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void CreateOneProductTest()
        {
            string productName = UniqueName();
            bool productExist = false;

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
            driver.FindElement(By.CssSelector("div#tab-general input[name='name[en]']")).SendKeys(productName);
            //Code
            driver.FindElement(By.CssSelector("div#tab-general input[name='code']")).SendKeys("12345");
            //Category
            driver.FindElement(By.CssSelector("div#tab-general input[data-name='Rubber Ducks']")).Click();
            //Prodduct Group
            driver.FindElement(By.CssSelector("div#tab-general input[name*='product_groups'][value='1-1']")).Click();
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
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("div#tab-information")));
            //Selectc Manufacture
            var selectManufacture = new SelectElement(driver.FindElement(By.CssSelector("div#tab-information select[name='manufacturer_id'")));
            selectManufacture.SelectByValue("1");
            //Keywords
            driver.FindElement(By.CssSelector("div#tab-information input[name='keywords']")).SendKeys("blue, duck");
            //Short description
            driver.FindElement(By.CssSelector("div#tab-information input[name*='short_description']")).SendKeys("Moden blue duck");
            //Description
            driver.FindElement(By.CssSelector("div#tab-information div.trumbowyg-editor")).SendKeys("blue, duck");
            //Head title
            driver.FindElement(By.CssSelector("div#tab-information input[name*='head_title']")).SendKeys("Blue duck");
            //Meta description
            driver.FindElement(By.CssSelector("div#tab-information input[name*='meta_description[en]']")).SendKeys("Blue duck");

            //Fill in Prices
            //Go to Prices tab
            driver.FindElement(By.CssSelector("div.tabs a[href*='prices']")).Click();
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("div#tab-prices")));
            //Purchase price
            driver.FindElement(By.CssSelector("div#tab-prices input[name='purchase_price']")).Clear();
            driver.FindElement(By.CssSelector("div#tab-prices input[name='purchase_price']")).SendKeys("15");
            var selectCurrency = new SelectElement(driver.FindElement(By.CssSelector("div#tab-prices select[name='purchase_price_currency_code']")));
            selectCurrency.SelectByValue("USD");
            //Price
            driver.FindElement(By.CssSelector("div#tab-prices input[name='prices[USD]']")).SendKeys("10");
            driver.FindElement(By.CssSelector("div#tab-prices input[name='prices[EUR]']")).SendKeys("9");
            //Click button Save
            wait.Until(ExpectedConditions.ElementToBeClickable(driver.FindElement(By.Name("save")))).Click();
           
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("form[name='catalog_form']")));
            var products = driver.FindElements(By.CssSelector("form[name='catalog_form'] a:not([title='Edit'])"));
            foreach(var pr in products)
            {
                if (!String.IsNullOrEmpty(pr.GetAttribute("textContent")))
                    if (pr.GetAttribute("textContent").Equals(productName))
                    {
                        productExist = true;
                        break;
                    }
            }
            Assert.True(productExist);


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

        private string UniqueName()
        {
            Random random = new Random();
            return string.Format("TestProduct{0:0000}", random.Next(10000));
        }
    }
}
