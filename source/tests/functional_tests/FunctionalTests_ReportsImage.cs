using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace functional_tests
{
    internal class FunctionalTests_ReportsImage
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }
        private IJavaScriptExecutor js;
        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            js = (IJavaScriptExecutor)driver;
            vars = new Dictionary<string, object>();
        }
        [TearDown]
        protected void TearDown()
        {
            driver.Quit();
        }
        [Test]
        public void detailsRecordImage()
        {
            driver.Navigate().GoToUrl("https://localhost:5064/");
            driver.Manage().Window.Size = new System.Drawing.Size(1552, 840);
            driver.FindElement(By.LinkText("Iniciar Sesión")).Click();
            driver.FindElement(By.Id("Input_UserName")).Click();
            driver.FindElement(By.Id("Input_UserName")).SendKeys("anne");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Anne1.");
            driver.FindElement(By.CssSelector(".register_submit")).Click();
            driver.FindElement(By.Id("SearchString")).Click();
            driver.FindElement(By.Id("SearchString")).SendKeys("ca");
            driver.FindElement(By.Id("searchButton")).Click();
            driver.FindElement(By.LinkText("Calcomania")).Click();
            var elements = driver.FindElements(By.CssSelector(".active > .d-block"));
            Assert.True(elements.Count > 0);
        }

        [Test]
        public void detailsRecordImage2()
        {
            driver.Navigate().GoToUrl("https://localhost:5064/");
            driver.Manage().Window.Size = new System.Drawing.Size(1552, 840);
            driver.FindElement(By.LinkText("Iniciar Sesión")).Click();
            driver.FindElement(By.Id("Input_UserName")).Click();
            driver.FindElement(By.Id("Input_UserName")).SendKeys("anne");
            driver.FindElement(By.Id("Input_Password")).SendKeys("Anne1.");
            driver.FindElement(By.CssSelector(".register_submit")).Click();
            driver.FindElement(By.Id("SearchString")).Click();
            driver.FindElement(By.Id("SearchString")).SendKeys("ca");
            driver.FindElement(By.Id("searchButton")).Click();
            driver.FindElement(By.LinkText("Calcomania")).Click();
            driver.FindElement(By.CssSelector("tr:nth-child(1) .btn-toggle-images")).Click();
            var elements = driver.FindElements(By.CssSelector("#imagesCollapse2023-11-26-17-04-15 > .d-block"));
            Assert.True(elements.Count > 0);
        }
    }
}
