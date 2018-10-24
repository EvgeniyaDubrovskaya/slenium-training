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
            OpenMainPage(driver);
            //Add 3 Product to Basket
            for(int i = 0; i < 3; i++)
            {
                OpenProductPage(i);
                
                //Get counter for products in basket
                //var wbCountPr = driver.FindElement(By.CssSelector("div#cart a span.quantity"));
                //Add product to basket
                AddProductToBasket();
                //Counter is updated
                //wait.Until(ExpectedConditions.TextToBePresentInElement(wbCountPr, (i+1).ToString()));
                //Return on main page
                //if (i < 2)
                //{
                //    OpenProductPage(i);                    
                //}
            }
            //Go to checkout page
            OpenBasketPage();

            //Get amount of products in basket
            //var prCount = driver.FindElements(By.CssSelector("div#checkout-summary-wrapper td.item")).Count; 
            var prCount = CountProductOnCheckoutPage();
            do
            {
                //if (IsElementPresent(driver, By.CssSelector("ul.shortcuts")))
                //{
                //    wait.Until(ExpectedConditions.ElementExists(By.CssSelector("li.shortcut a"))).Click();
                //}
                //Remove product
                //wait.Until(ExpectedConditions.ElementExists(By.CssSelector("li.item button[name='remove_cart_item']"))).Click();
                RemoveProductFromBasket();
                //Update amount of product in basket
                prCount--;
                //if (prCount != 0)
                //    wait.Until(d => driver.FindElements(By.CssSelector("div#checkout-summary-wrapper td.item")).Count == prCount);
            } while(prCount > 0);

            IWebElement wb = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("div#checkout-cart-wrapper p em")));
            Assert.IsTrue(wb.GetAttribute("textContent") == "There are no items in your cart.");
            
        }

        public void RemoveProductFromBasket()
        {
            var prCount = CountProductOnCheckoutPage();
            if (IsElementPresent(driver, By.CssSelector("ul.shortcuts")))
                {
                wait.Until(ExpectedConditions.ElementExists(By.CssSelector("li.shortcut a"))).Click();
            }
            //Remove product
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("li.item button[name='remove_cart_item']"))).Click();
            if (prCount > 0)
                wait.Until(d => CountProductOnCheckoutPage() == prCount - 1);
        }

        public int CountProductOnCheckoutPage()
        {

            return driver.FindElements(By.CssSelector("div#checkout-summary-wrapper td.item")).Count;
        }
        public int CountProductInBasket(IWebDriver driver)
        {
            return int.Parse(driver.FindElement(By.CssSelector("div#cart a span.quantity")).Text);
        }

        public void AddProductToBasket()
        {
            var wbCountPrOld = CountProductInBasket(driver);
            if (IsElementPresent(driver, By.CssSelector("select[name='options[Size]'")))
            {
                SelectElement select = new SelectElement(driver.FindElement(By.CssSelector("select[name='options[Size]'")));
                select.SelectByIndex(1);
            }
            driver.FindElement(By.CssSelector("button[name='add_cart_product']")).Click();
            //wait.Until(ExpectedConditions.TextToBePresentInElementValue(driver.FindElement(By.CssSelector("div#cart a span.quantity")), (wbCountPrOld + 1).ToString()));
            wait.Until(d => CountProductInBasket(driver) == (wbCountPrOld + 1));
        }

        public void OpenBasketPage()
        {
            driver.FindElement(By.CssSelector("div#cart a.link")).Click();
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("div#checkout-cart-wrapper")));
        }
        public void OpenMainPage(IWebDriver driver)
        {            
            //Open main page as user
            driver.Url = "http://localhost/litecart/";
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("div#box-most-popular")));
            // wait.Until(_driver => _driver.Title.ToString().Equals("Online Store | My Store"));
        }

        public void OpenProductPage(int prNumber)
        {
            var wbCards = driver.FindElements(By.CssSelector("li.product"));
            //Go to Product page
            wbCards[prNumber].FindElement(By.CssSelector("a")).Click();
            //Wait button Add product
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("button[name='add_cart_product']")));
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
