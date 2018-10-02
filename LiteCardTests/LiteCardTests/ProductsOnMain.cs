using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCardTests
{
    [TestFixture]
    class ProductsOnMain
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void start()
        {
            driver = new FirefoxDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void ProductHasStiker()
        {
            //Open main page as user
            driver.Url = "http://localhost/litecart/";
            wait.Until(driver => driver.Title.ToString().Equals("Online Store | My Store"));

            //Get amount of product card on main page
            var count = driver.FindElements(By.CssSelector("li.product")).Count();

            for(int i = 0; i < count; i ++)
            {
                IWebElement productCard = driver.FindElements(By.CssSelector("li.product")).ElementAt(i);
                Assert.True(productCard.FindElements(By.CssSelector("div.sticker")).Count() == 1);
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
