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

        // Test Funcional: James Araya Rodríguez. Sprint 3
        [Test]
        public void vistaImagen()
        {
            driver.Navigate().GoToUrl("http://localhost:5064/");
            driver.Manage().Window.Size = new System.Drawing.Size(1206, 824);
            driver.FindElement(By.Id("SearchString")).Click();
            driver.FindElement(By.Id("SearchString")).SendKeys("Fun");
            driver.FindElement(By.Id("searchButton")).Click();
            driver.FindElement(By.LinkText("Funko Pop")).Click();
            var elements = driver.FindElements(By.CssSelector(".active > .d-block"));
            Assert.True(elements.Count > 0);
        }

        // Test Funcional: James Araya Rodríguez. Sprint 3
        [Test]
        public void vistaSinImagen()
        {
            driver.Navigate().GoToUrl("http://localhost:5064/");
            driver.Manage().Window.Size = new System.Drawing.Size(1206, 824);
            driver.FindElement(By.Id("SearchString")).Click();
            driver.FindElement(By.Id("SearchString")).SendKeys("Col");
            driver.FindElement(By.Id("searchButton")).Click();
            driver.FindElement(By.LinkText("Colgate Pasta Total 12 Clean Mint 75ml")).Click();
            var elements = driver.FindElements(By.CssSelector(".sinImagen > img"));
            Assert.True(elements.Count > 0);
        }
    }
}
