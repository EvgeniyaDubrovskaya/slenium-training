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
    class Basket
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void start()
        {
            driver = new ChromeDriver();
            //driver = new FirefoxDriver();
            //InternetExplorerOptions options = new InternetExplorerOptions();
            //options.RequireWindowFocus = true;
            //driver = new InternetExplorerDriver(options);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void AddDeleteProductsInBasket()
        {
            //Open main page as user
            driver.Url = "http://localhost/litecart/";
            wait.Until(driver => driver.Title.ToString().Equals("Online Store | My Store"));

            //Add 3 Product to Basket
            for(int i = 0; i < 3; i++)
            {
                //Get products on main page
                var wbCards = driver.FindElements(By.CssSelector("li.product"));
                //Go to Product page
                wbCards[i].FindElement(By.CssSelector("a")).Click();
                var wbbuttion = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("button[name='add_cart_product']")));
                //Get counter for products in basket
                var wbCountPr = driver.FindElement(By.CssSelector("div#cart a span.quantity"));
                //Add product to basket
                if(IsElementPresent(driver,By.CssSelector("select[name='options[Size]'")))
                {
                    SelectElement select = new SelectElement(driver.FindElement(By.CssSelector("select[name='options[Size]'")));
                    select.SelectByIndex(1);
                }
                wbbuttion.Click();
                //Counter is updated
               // wait.Until(d => driver.FindElement(By.CssSelector("div#cart a span.quantity")).GetAttribute("textContent") + 1);
                wait.Until(ExpectedConditions.TextToBePresentInElement(wbCountPr, (i+1).ToString()));
                //Return on main page
                if (i < 2)
                {
                    driver.FindElement(By.CssSelector("nav#site-menu li.general-0 a")).Click();
                    wait.Until(ExpectedConditions.ElementExists(By.CssSelector("div#box-most-popular")));
                }
            }
            //Go to checkout page
            driver.FindElement(By.CssSelector("div#cart a.link")).Click();
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("div#checkout-cart-wrapper")));

            //Get amount of products in basket
            var prCount = driver.FindElements(By.CssSelector("div#checkout-summary-wrapper td.item")).Count; 
            do
            {
                if (IsElementPresent(driver, By.CssSelector("ul.shortcuts")))
                {
                    wait.Until(ExpectedConditions.ElementExists(By.CssSelector("li.shortcut a"))).Click();
                }
                //Remove product
                wait.Until(ExpectedConditions.ElementExists(By.CssSelector("li.item button[name='remove_cart_item']"))).Click();
                //Update amount of product in basket
                prCount--;
                if (prCount != 0)
                    wait.Until(d => driver.FindElements(By.CssSelector("div#checkout-summary-wrapper td.item")).Count == prCount);
            } while(prCount > 0);

            IWebElement wb = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("div#checkout-cart-wrapper p em")));
            Assert.IsTrue(wb.GetAttribute("textContent") == "There are no items in your cart.");
            
        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }

        private bool IsElementPresent(IWebDriver driver, By locator)
        {
            try
            {
                driver.FindElement(locator);
                return true;
            }
            catch (NoSuchElementException ex)
            {
                return false;
            }
        }
    }
}
