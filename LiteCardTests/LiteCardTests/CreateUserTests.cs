using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;

namespace LiteCardTests
{
    [TestFixture]
    class CreateUserTests
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
        public void RegistrationNewUserTest()
        {
            //Open main page as user
            driver.Url = "http://localhost/litecart/";
            wait.Until(driver => driver.Title.ToString().Equals("Online Store | My Store"));

            //Go to link for new customers
            driver.Url = driver.FindElement(By.CssSelector("form[name='login_form'] a")).GetAttribute("href");
            wait.Until(driver => driver.Title.ToString().Equals("Create Account | My Store"));

            //Fill in new user form
            driver.FindElement(By.CssSelector("input[name='firstname']")).SendKeys("Jane");
            driver.FindElement(By.CssSelector("input[name='lastname']")).SendKeys("Doy");
            driver.FindElement(By.CssSelector("input[name='address1']")).SendKeys("address1");
            driver.FindElement(By.CssSelector("input[name='address2']")).SendKeys("address2");
            driver.FindElement(By.CssSelector("input[name='postcode']")).SendKeys("12345");
            driver.FindElement(By.CssSelector("input[name='city']")).SendKeys("LA");
            //Select country
            var wbCountry = new SelectElement(driver.FindElement(By.CssSelector("select[name='country_code']")));
            wbCountry.SelectByText("Albania");
            //Select state
            var wbState = new SelectElement(driver.FindElement(By.CssSelector("select[name='country_zone']")));
            wbState.SelectByValue("Alabama");
            //Email
            driver.FindElement(By.CssSelector("input[name='email']")).SendKeys("??");
            driver.FindElement(By.CssSelector("input[name='phone']")).SendKeys("89522415");
            
        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
