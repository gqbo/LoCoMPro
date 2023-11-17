using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace functional_tests
{
    [TestFixture]
    public class FunctionalTest_Reports
    {
        private IWebDriver driver;
        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
        }
        [TearDown]
        protected void TearDown()
        {
            driver.Quit();
        }

        // Test Funcional: James Araya Rodríguez. Sprint 2
        [Test]
        public void ReportsFunctionalTest()
        {
            driver.Navigate().GoToUrl("http://localhost:5064/");
            driver.FindElement(By.LinkText("Iniciar Sesión")).Click();
            driver.FindElement(By.Id("Input_UserName")).SendKeys("anne");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.CssSelector(".row:nth-child(3) > .col")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Anne1.");
            driver.FindElement(By.CssSelector(".register_submit")).Click();
            driver.FindElement(By.Id("SearchString")).Click();
            driver.FindElement(By.Id("SearchString")).SendKeys("Pan");
            driver.FindElement(By.Id("searchButton")).Click();
            driver.FindElement(By.LinkText("Pantalón bershka gris")).Click();
            driver.FindElement(By.CssSelector("tr:nth-child(1) img")).Click();
            driver.FindElement(By.Id("Report_Comment")).Click();
            driver.FindElement(By.Id("Report_Comment")).SendKeys("El pantalón no tenía descuento.");
            driver.FindElement(By.Id("ReportButton")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(5000);
            string currentUrl = driver.Url;

            string expectedUrl = "http://localhost:5064/";

            Assert.That(currentUrl, Is.EqualTo(expectedUrl));
        }
    }
}