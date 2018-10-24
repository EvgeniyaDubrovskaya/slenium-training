using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCardTests.pages
{
    internal class CheckoutPage: Page
    {
        public CheckoutPage(IWebDriver driver) : base(driver) { }

        internal CheckoutPage Open()
        {
            driver.FindElement(By.CssSelector("div#cart a.link")).Click();
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("div#checkout-cart-wrapper")));
            return this;
        }

        public int CountProductOnCheckoutPage()
        {
            return driver.FindElements(By.CssSelector("div#checkout-summary-wrapper td.item")).Count;
        }

        public void RemoveProduct()
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
    }
}
