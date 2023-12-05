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
        [Test]
        public void anomaliesDateRefused()
        {
            driver.Navigate().GoToUrl("https://localhost:5064/");
            driver.Manage().Window.Size = new System.Drawing.Size(1552, 840);
            driver.FindElement(By.LinkText("Iniciar Sesión")).Click();
            driver.FindElement(By.Id("Input_UserName")).Click();
            driver.FindElement(By.Id("Input_UserName")).SendKeys("anne");
            driver.FindElement(By.Id("Input_Password")).SendKeys("Anne1.");
            driver.FindElement(By.CssSelector(".register_submit")).Click();
            driver.FindElement(By.Id("trigger1")).Click();
            driver.FindElement(By.LinkText("Ver Anomalías")).Click();
            driver.FindElement(By.Id("anomalies-date")).Click();
            driver.FindElement(By.LinkText("Camisa original Real Madrid #5 Bellingham")).Click();
            driver.FindElement(By.CssSelector(".add_cancel")).Click();
            driver.FindElement(By.Id("anomalies-date")).Click();
            var elements = driver.FindElements(By.LinkText("Camisa original Real Madrid #5 Bellingham"));
            Assert.True(elements.Count > 0);
        }


        
    
    }
}

