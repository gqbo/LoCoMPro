﻿using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

namespace functional_tests
{
    [TestFixture]
    public class FunctionalTest_GeolocationStore
    {
        private IWebDriver driver;
        private IJavaScriptExecutor js;
        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            js = (IJavaScriptExecutor)driver;
        }
        [TearDown]
        protected void TearDown()
        {
            driver.Quit();
        }

        // Test Funcional: Yordi Robles Siles. Sprint 2
        [Test]
        public void GeolocationFunctionalTest()
        {
            driver.Navigate().GoToUrl("http://localhost:5064/");
            driver.FindElement(By.LinkText("Iniciar Sesión")).Click();
            driver.FindElement(By.Id("Input_UserName")).SendKeys("yordi");
            driver.FindElement(By.Id("Input_Password")).SendKeys("Yordi1.");
            driver.FindElement(By.CssSelector(".row:nth-child(2) > .col")).Click();
            driver.FindElement(By.CssSelector(".register_submit")).Click();
            driver.FindElement(By.Id("AgregarProducto")).Click();
            {
                var element = driver.FindElement(By.Id("map"));
                Actions builder = new Actions(driver);
                builder.MoveToElement(element).ClickAndHold().Perform();
            }
            {
                var element = driver.FindElement(By.Id("map"));
                Actions builder = new Actions(driver);
                builder.MoveToElement(element).Perform();
            }
            {
                var element = driver.FindElement(By.Id("map"));
                Actions builder = new Actions(driver);
                builder.MoveToElement(element).Release().Perform();
            }
            driver.FindElement(By.Id("map")).Click();
            {
                var element = driver.FindElement(By.CssSelector(".leaflet-marker-icon"));
                Actions builder = new Actions(driver);
                builder.MoveToElement(element).ClickAndHold().Perform();
            }
            {
                var element = driver.FindElement(By.CssSelector(".leaflet-marker-icon"));
                Actions builder = new Actions(driver);
                builder.MoveToElement(element).Perform();
            }
            {
                var element = driver.FindElement(By.CssSelector(".leaflet-marker-icon"));
                Actions builder = new Actions(driver);
                builder.MoveToElement(element).Release().Perform();
            }
            driver.FindElement(By.CssSelector(".leaflet-marker-icon")).Click();
            driver.FindElement(By.Id("Store")).Click();
            driver.FindElement(By.Id("Store")).SendKeys("Pan Suavecito");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(50000);
            driver.FindElement(By.CssSelector(".add_confirm")).Click();
            string currentUrl = driver.Url;
            string expectedUrl = "http://localhost:5064/Records/Create?latitude=9.9281&longitude=-84.0907&nameStore=Pan%20Suavecito&nameProvince=San%20Jos%C3%A9&nameCanton=San%20Jos%C3%A9";
            Assert.That(currentUrl, Is.EqualTo(expectedUrl));
        }
    }
}
