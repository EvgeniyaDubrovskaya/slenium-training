using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
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
            //driver = new ChromeDriver();
            //driver = new FirefoxDriver();
            InternetExplorerOptions options = new InternetExplorerOptions();
            options.RequireWindowFocus = true;
            driver = new InternetExplorerDriver(options);
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
            driver.FindElement(By.CssSelector("input[name='city']")).SendKeys("LA");
            //Select country
            driver.FindElement(By.CssSelector("span.select2-selection__arrow")).Click();
            driver.FindElement(By.CssSelector("input.select2-search__field")).SendKeys("United States");
            driver.FindElement(By.CssSelector("span.select2-results li[id*='US']")).Click();
            //Select state
            var state = new SelectElement(driver.FindElement(By.CssSelector("select[name='zone_code']")));
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("select[name='zone_code'] option")));
            state.SelectByIndex(1);

            //Email
            var adress = UniqueEmail();
            driver.FindElement(By.CssSelector("input[name='email']")).SendKeys(adress);
            driver.FindElement(By.CssSelector("input[name='phone']")).SendKeys("89522415");
            driver.FindElement(By.CssSelector("input[name='postcode']")).SendKeys("12345");
            //Password
            driver.FindElement(By.CssSelector("input[name='password']")).SendKeys("password");
            driver.FindElement(By.CssSelector("input[name='confirmed_password']")).SendKeys("password");

            driver.FindElement(By.CssSelector("button[name='create_account']")).Click();
            wait.Until(driver => driver.FindElement(By.CssSelector("div#box-account h3")).GetAttribute("textContent").ToString().Equals("Account"));

            //Logout
            driver.FindElement(By.CssSelector("div#box-account li a[href*='logout'")).Click();
            wait.Until(driver => driver.Title.ToString().Equals("Online Store | My Store"));

            //Login as new user
            driver.FindElement(By.CssSelector("form input[name='email']")).SendKeys(adress);
            driver.FindElement(By.CssSelector("form input[name='password']")).SendKeys("password");
            driver.FindElement(By.CssSelector("form button[name='login']")).Click();
            wait.Until(driver => driver.FindElement(By.CssSelector("div#box-account h3")).GetAttribute("textContent").ToString().Equals("Account"));
            //Logout
            driver.FindElement(By.CssSelector("div#box-account li a[href*='logout'")).Click();
            wait.Until(driver => driver.Title.ToString().Equals("Online Store | My Store"));
        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }

        private string UniqueEmail()
        {
            Random random = new Random();
            return string.Format("qa{0:0000}@test.com", random.Next(10000));
        }
    }
}
