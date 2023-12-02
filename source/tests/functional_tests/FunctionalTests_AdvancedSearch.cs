using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace functional_tests
{
    public class FunctionalTest_AdvancedSearch
    {
        IWebDriver driver;
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
        }

        [TearDown]
        protected void TearDown()
        {
            driver.Quit();
        }

        // Test Funcional: Sebastián Rodríguez Tencio. Sprint 3
        [Test]
        public void advancedSearchTest()
        {
            driver.Navigate().GoToUrl("https://localhost:5064/");
            driver.Manage().Window.Size = new System.Drawing.Size(968, 1079);
            driver.FindElement(By.CssSelector(".advanced_search")).Click();
            driver.FindElement(By.Id("Province")).Click();
            {
                var dropdown = driver.FindElement(By.Id("Province"));
                dropdown.FindElement(By.XPath("//option[. = 'San José']")).Click();
            }
            driver.FindElement(By.Id("Canton")).Click();
            {
                var dropdown = driver.FindElement(By.Id("Canton"));
                dropdown.FindElement(By.XPath("//option[. = 'Tibás']")).Click();
            }
            driver.FindElement(By.Id("Category")).Click();
            {
                var dropdown = driver.FindElement(By.Id("Category"));
                dropdown.FindElement(By.XPath("//option[. = 'Celulares']")).Click();
            }
            driver.FindElement(By.Id("SearchString")).Click();
            driver.FindElement(By.Id("SearchString")).SendKeys("Apple");
            driver.FindElement(By.Id("searchButton")).Click();

            Assert.IsTrue(driver.PageSource.Contains("Apple") &&
              driver.PageSource.Contains("San José") &&
              driver.PageSource.Contains("Tibás") &&
              driver.PageSource.Contains("Celulares"));
        }
    }
}