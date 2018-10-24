using LiteCardTests.tests;
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

namespace LiteCardTests
{
    [TestFixture]
    public class BasketTest: TestBase
    {
        [Test]
        public void AddDeleteProductsInBasket()
        {

            for(int i = 0; i < 3; i++)
            {
                app.AddProduct(i);
            }

            app.RemoveAllProduct();

            Assert.IsTrue(app.GetNotificationText() == "There are no items in your cart.");            
        }          
    }
}
