using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace functional_tests
{
    internal class functionalTest_AnomaliesDate
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
        public void anomaliasDate()
        {
            driver.Navigate().GoToUrl("http://localhost:5064/");
            driver.Manage().Window.Size = new System.Drawing.Size(1051, 806);
            driver.FindElement(By.LinkText("Iniciar Sesión")).Click();
            driver.FindElement(By.Id("Input_UserName")).Click();
            driver.FindElement(By.Id("Input_UserName")).SendKeys("Admin");
            driver.FindElement(By.Id("Input_Password")).SendKeys("Admin1.");
            driver.FindElement(By.CssSelector(".register_submit")).Click();
            driver.FindElement(By.Id("trigger1")).Click();
            driver.FindElement(By.LinkText("Anomalías")).Click();
            driver.FindElement(By.LinkText("Funko Pop")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(50000);
            driver.FindElement(By.CssSelector(".add_cancel")).Click();
            driver.FindElement(By.Id("anomalies-date")).Click();
            Assert.That(driver.FindElement(By.LinkText("Funko Pop")).Text, Is.EqualTo("Funko Pop"));
        }
    }
}

