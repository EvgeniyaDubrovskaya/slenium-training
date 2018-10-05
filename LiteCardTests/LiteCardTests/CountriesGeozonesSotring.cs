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
    class CountriesGeozonesSotring
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
        public void CountriesSortingTest()
        {
            //Login as admin
            driver.Url = "http://localhost/litecart/admin/";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();
            wait.Until(driver => driver.Title.ToString().Equals("My Store"));

            //Go to Countries page
            driver.Url = "http://localhost/litecart/admin/?app=countries&doc=countries";
            wait.Until(driver => driver.Title.ToString().Equals("Countries | My Store"));

            //Get all countries
            var wbCountries = driver.FindElements(By.CssSelector("tr.row td a:not([title=Edit])"));
            List<string> countries = new List<string>();

            foreach (var wbCountiry in wbCountries)
            {
                countries.Add(wbCountiry.GetAttribute("textContent"));
            }
            
            Assert.True(isListSorted(countries));
        }


        [Test]
        public void GeoZoneSortingTest()
        {
            //Login as admin
            driver.Url = "http://localhost/litecart/admin/";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();
            wait.Until(driver => driver.Title.ToString().Equals("My Store"));

            //Go to Countries page
            driver.Url = "http://localhost/litecart/admin/?app=countries&doc=countries";
            wait.Until(driver => driver.Title.ToString().Equals("Countries | My Store")); 

            //Get all countries
            var wbRows = driver.FindElements(By.CssSelector("tr.row"));
            List<string> urls = new List<string>();

            //Get countries uls wich has Geozone
            foreach (var wbRow in wbRows)
            {
                if (wbRow.FindElement(By.CssSelector(":nth-child(6)")).GetAttribute("textContent") != "0")
                    urls.Add(wbRow.FindElement(By.CssSelector("a:not([title=Edit]")).GetAttribute("href"));
            }

            foreach (var url in urls)
            {
                driver.Url = url;
                wait.Until(driver => driver.Title.ToString().Contains("My Store"));
                var wbGeoZones = driver.FindElements(By.CssSelector("#table-zone td input[name*='name']"));

                List<string> zones = new List<string>();
                foreach (var wbGeoZone in wbGeoZones)
                {
                    zones.Add(wbGeoZone.GetAttribute("value"));
                }
                Assert.True(isListSorted(zones));
            }
            
        }


        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }

        private bool isListSorted(List<string> stList)
        {
            for (int i = 0; i < stList.Count - 1; i++)
            {
               if (stList[i].CompareTo(stList[i+1]) > 0)
                    return false;                
            }
            return true;

        }

    }
}
