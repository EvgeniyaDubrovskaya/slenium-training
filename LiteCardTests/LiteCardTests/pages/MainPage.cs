using LiteCardTests.pages;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCardTests.app
{
    class MainPage: Page
    {
        public MainPage(IWebDriver driver): base(driver) { }
        
        internal void Open()
        {
            driver.Url = "http://localhost/litecart/";
            // return this;
        }


    }
}
