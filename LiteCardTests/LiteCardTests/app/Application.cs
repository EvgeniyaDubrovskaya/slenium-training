using LiteCardTests.pages;
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
    public class Application
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        private MainPage mainPage;
        private ProductPage productPage;
        private CheckoutPage checkoutPage;


        public Application()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            mainPage = new MainPage(driver);
            productPage = new ProductPage(driver);
            checkoutPage = new CheckoutPage(driver);
        }

        internal void AddProduct(int prNumber)
        {
            mainPage.Open();
            productPage.Open(prNumber);
            productPage.AddProductToBasket();
        }

        internal void RemoveAllProduct()
        {
            checkoutPage.Open();
            //Get amount of products in basket
            var prCount = checkoutPage.CountProductOnCheckoutPage();

            do
            {
                //Remove product
                checkoutPage.RemoveProduct();
                //Update amount of product in basket
                prCount--;
            } while (prCount > 0);
        }        

        public string GetNotificationText()
        {
            IWebElement wb = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("div#checkout-cart-wrapper p em")));
            return wb.GetAttribute("textContent");
        }

        public void Quit()
        {
            driver.Quit();
        }
    }
}
