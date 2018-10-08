using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LiteCardTests
{
    [TestFixture]
    class ProductCardTests
    {

        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void start()
        {
            //driver = new FirefoxDriver();
            //driver = new ChromeDriver();
            InternetExplorerOptions options = new InternetExplorerOptions();
            options.RequireWindowFocus = true;
            driver = new InternetExplorerDriver(options);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void ProductCardViewTest()
        {
            List<ProductCartValue> prValues = new List<ProductCartValue>();
            
            //Open main page as user
            driver.Url = "http://localhost/litecart/";
            wait.Until(driver => driver.Title.ToString().Equals("Online Store | My Store"));

            //Get amount of product card on main page
            var wbCards = driver.FindElements(By.CssSelector("li.product"));

            foreach (var wbCard in wbCards)
            {
                ProductCartValue prValue = new ProductCartValue();
                prValue.url = wbCard.FindElement(By.CssSelector("a.link")).GetAttribute("href");
                prValue.name = wbCard.FindElement(By.CssSelector("div.name")).GetAttribute("textContent");

                //Product not on sale
                if (IsElementPresent(wbCard, By.CssSelector(".price")))
                {
                    prValue.price = wbCard.FindElement(By.CssSelector(".price")).GetAttribute("textContent");
                    prValue.promotion = "";
                }
                //Product on sale
                else
                {
                    prValue.price = wbCard.FindElement(By.CssSelector(".regular-price")).GetAttribute("textContent");
                    prValue.promotion = wbCard.FindElement(By.CssSelector(".campaign-price")).GetAttribute("textContent");

                    //Regular price on Product card is Gray o Main Page
                    Assert.True(isGrayColour(wbCard.FindElement(By.CssSelector(".regular-price")).GetCssValue("colour")));
                    //Campaign price on Product card is Red o Main Page
                    Assert.True(isRedColour(wbCard.FindElement(By.CssSelector(".campaign-price")).GetCssValue("colour")));
                    //Product regular price is Strike
                    Assert.True(wbCard.FindElement(By.CssSelector(".regular-price")).TagName == "s");
                    //Product campaign price is Bold
                    Assert.True(wbCard.FindElement(By.CssSelector(".campaign-price")).TagName == "strong");
                    //Campaign price is bigger than regular
                    var fzRegualr = getFontSize(wbCard.FindElement(By.CssSelector(".regular-price")).GetCssValue("font-size"));
                    var fzCampaign = getFontSize(wbCard.FindElement(By.CssSelector(".campaign-price")).GetCssValue("font-size"));
                    Assert.IsTrue(fzCampaign > fzRegualr);

                }

                prValues.Add(prValue);
            }

            foreach(var prValue in prValues)
            {
                driver.Url = prValue.url;
                wait.Until(driver => driver.Title.ToString().Contains("My Store"));
                //Product name is the same as on main page
                Assert.IsTrue(driver.FindElement(By.CssSelector("h1.title")).GetAttribute("textContent").Equals(prValue.name));

                if(IsElementPresent(driver.FindElement(By.CssSelector("div.information")), By.CssSelector(".price")))
                {
                    //Product price is the same as on main page
                    Assert.IsTrue(driver.FindElement(By.CssSelector("div.information .price")).GetAttribute("textContent").Equals(prValue.price));
      
                }
                else
                {
                    //Product regular price is the same as on main page
                    Assert.IsTrue(driver.FindElement(By.CssSelector("div.information .regular-price")).GetAttribute("textContent").Equals(prValue.price));
                    //Product campaign price is the same as on main page
                    Assert.IsTrue(driver.FindElement(By.CssSelector("div.information .campaign-price")).GetAttribute("textContent").Equals(prValue.promotion));
                    //Product regular price is Gray
                    Assert.IsTrue(isGrayColour(driver.FindElement(By.CssSelector("div.information .regular-price")).GetCssValue("colour")));
                    //Product campaign price is Red
                    Assert.IsTrue(isRedColour(driver.FindElement(By.CssSelector("div.information .campaign-price")).GetCssValue("colour")));
                    //Product regular price is ---
                    Assert.True(driver.FindElement(By.CssSelector("div.information .regular-price")).TagName == "s");
                    //Product campaign price is Bold
                    Assert.True(driver.FindElement(By.CssSelector("div.information .campaign-price")).TagName == "strong");
                    //Campaign price is bigger than regular
                    var fzRegualr = getFontSize(driver.FindElement(By.CssSelector("div.information .regular-price")).GetCssValue("font-size"));
                    var fzCampaign = getFontSize(driver.FindElement(By.CssSelector("div.information .campaign-price")).GetCssValue("font-size"));
                    Assert.IsTrue(fzCampaign > fzRegualr);

                }

            }

        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }

        private bool IsElementPresent(IWebElement webElement, By locator)
        {
            try
            {
                webElement.FindElement(locator);
                return true;
            }
            catch (NoSuchElementException ex)
            {
                return false;
            }
        }

        private int[] ColourParse(string colour)
        {
            int[] RGB = new int[3];

            System.Text.RegularExpressions.Regex regex = new Regex(@"rgb\((?<r>\d{1,3}),(?<g>\d{1,3}),(?<b>\d{1,3})\)");
            Match match = regex.Match(colour);
            if (match.Success)
            {
                RGB[0] = int.Parse(match.Groups["r"].Value);
                RGB[1] = int.Parse(match.Groups["g"].Value);
                RGB[2] = int.Parse(match.Groups["b"].Value);
            }
            return RGB;
        }

        private bool isGrayColour(string colour)
        {
            int[] RGB = new int[3];
            RGB = ColourParse(colour);

            if (RGB[0] != RGB[1]) 
                return false;
            if (RGB[0] != RGB[2])
                return false;
            return true;
        }

        private bool isRedColour(string colour)
        {
            int[] RGB = new int[3];
            RGB = ColourParse(colour);

            if (RGB[1] != 0)
                return false;
            if (RGB[2] != 0)
                return false;
            return true;
        }

        private double getFontSize(string sf)
        {
            System.Text.RegularExpressions.Regex regex = new Regex(@"px");
            sf = regex.Replace(sf, String.Empty);
            return Convert.ToDouble(sf, CultureInfo.InvariantCulture);
        }
    }

    struct ProductCartValue
    {
        public string url;
        public string name;
        public string price;
        public string promotion;
    }


}
