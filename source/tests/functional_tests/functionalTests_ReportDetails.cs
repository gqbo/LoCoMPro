using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace functional_tests
{
    public class FunctionalTest_ReportDetails
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

        // Test Funcional: Cristopher Hernández Calderón. Sprint 2
        [Test]
        public void ReportDetailsFunctionalTest()
        {
            driver.Navigate().GoToUrl("http://localhost:5064/");
            driver.Manage().Window.Size = new System.Drawing.Size(974, 1032);
            driver.FindElement(By.LinkText("Iniciar Sesión")).Click();
            driver.FindElement(By.Id("Input_UserName")).SendKeys("Admin");
            driver.FindElement(By.CssSelector(".row:nth-child(3) > .col")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Admin1.");
            driver.FindElement(By.CssSelector(".register_submit")).Click();
            driver.FindElement(By.LinkText("Ver reportes")).Click();
            driver.FindElement(By.LinkText("Hamburguesa con queso")).Click();
            string currentUrl = driver.Url;

            string expectedUrl = "http://localhost:5064/Reports/Details?NameGenerator=yordi&RecordDate=2023-01-18%2000%3A00%3A00";

            Assert.That(currentUrl, Is.EqualTo(expectedUrl));
        }
    }
}
