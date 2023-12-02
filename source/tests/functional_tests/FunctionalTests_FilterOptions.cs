using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace functional_tests
{
    public class FunctionalTests_FilterOptions
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
        public void FilterCheckOption_Test()
        {
            driver.Navigate().GoToUrl("https://localhost:5064/");
            driver.Manage().Window.Size = new System.Drawing.Size(968, 1079);
            driver.FindElement(By.Id("SearchString")).Click();
            driver.FindElement(By.Id("SearchString")).SendKeys("Apple");
            driver.FindElement(By.Id("searchButton")).Click();
            driver.FindElement(By.CssSelector("div:nth-child(8) > .province-checkbox")).Click();

            string expectedProvince = "San José";
            IReadOnlyCollection<IWebElement> productElements = driver.FindElements(By.CssSelector("tr[data-province='" + expectedProvince + "']"));

            Assert.IsTrue(productElements.Count > 0, $"No se encontraron productos relacionados con la provincia {expectedProvince} después de aplicar el filtro");
        }

        // Test Funcional: Sebastián Rodríguez Tencio. Sprint 3
        [Test]
        public void ClearFiltersButton_Test()
        {
            driver.Navigate().GoToUrl("https://localhost:5064/");
            driver.Manage().Window.Size = new System.Drawing.Size(968, 1079);
            driver.FindElement(By.Id("SearchString")).Click();
            driver.FindElement(By.Id("SearchString")).SendKeys("Apple");
            driver.FindElement(By.Id("searchButton")).Click();
            driver.FindElement(By.CssSelector("div:nth-child(8) > .province-checkbox")).Click();

            string expectedProvince = "San José";
            IReadOnlyCollection<IWebElement> productElements = driver.FindElements(By.CssSelector("tr[data-province='" + expectedProvince + "']"));
            driver.FindElement(By.Id("clear-filters")).Click();
            IReadOnlyCollection<IWebElement> provinceCheckboxes = driver.FindElements(By.CssSelector(".province-checkbox:checked"));

            Assert.That(0, Is.EqualTo(provinceCheckboxes.Count), "Los checkboxes de provincia no se desmarcaron correctamente");
        }
    }
}