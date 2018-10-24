using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCardTests.pages
{
    class ProductPage: Page
    {
        public ProductPage(IWebDriver driver) : base(driver) { }

        internal ProductPage Open(int prNumber)
        {
            var wbCards = driver.FindElements(By.CssSelector("li.product"));
            //Go to Product page
            wbCards[prNumber].FindElement(By.CssSelector("a")).Click();
            //Wait button Add product
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("button[name='add_cart_product']")));
            return this;
        }

        internal void AddProductToBasket()
        {
            var wbCountPrOld = CountProductInBasket();
            if (IsElementPresent(driver, By.CssSelector("select[name='options[Size]'")))
            {
                SelectElement select = new SelectElement(driver.FindElement(By.CssSelector("select[name='options[Size]'")));
                select.SelectByIndex(1);
            }
            driver.FindElement(By.CssSelector("button[name='add_cart_product']")).Click();
            //wait.Until(ExpectedConditions.TextToBePresentInElementValue(driver.FindElement(By.CssSelector("div#cart a span.quantity")), (wbCountPrOld + 1).ToString()));
            wait.Until(d => CountProductInBasket() == (wbCountPrOld + 1));
        }

        internal int CountProductInBasket()
        {
            return int.Parse(driver.FindElement(By.CssSelector("div#cart a span.quantity")).Text);
        }

    }
}
