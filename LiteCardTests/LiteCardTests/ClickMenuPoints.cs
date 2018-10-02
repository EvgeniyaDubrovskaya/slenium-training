using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCardTests
{
    [TestFixture]
    class ClickMenuPoints
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
        public void ClickMenu()
        {
            //Login as Admin
            driver.Url = "http://localhost/litecart/admin/";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();
            wait.Until(driver => driver.Title.ToString().Equals("My Store"));

            //Get amount of 1st level points from left menu
            var count1 = driver.FindElements(By.CssSelector("#app-")).Count();
            for (int i = 0; i < count1; i++)
            {
                //Get menu point and click on it
                IWebElement menuPoint = driver.FindElements(By.CssSelector("#app-")).ElementAt(i);
                menuPoint.Click();
                wait.Until(driver => driver.FindElement(By.CssSelector("h1")));
                
                //Get amount of 2st level points from left menu
                var count2 = driver.FindElements(By.CssSelector("#app- li")).Count();
                for(int j = 0; j < count2; j++)
                {
                    IWebElement submenuPoint = driver.FindElements(By.CssSelector("#app- li")).ElementAt(j);
                    submenuPoint.Click();
                    wait.Until(driver => driver.FindElement(By.CssSelector("h1")));
                }

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
