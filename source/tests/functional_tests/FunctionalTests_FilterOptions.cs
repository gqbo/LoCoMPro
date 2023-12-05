using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace functional_tests
{
    public class FunctionalTests_FilterOptions
    {
        IWebDriver driver;
        private FilterSearchPage searchPage;
        private FilterHomePage homePage;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            homePage = new FilterHomePage(driver);
            searchPage = new FilterSearchPage(driver);
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
            homePage.NavigateTo();
            searchPage.Search("Apple");
            driver.FindElement(By.CssSelector("div:nth-child(8) > .province-checkbox")).Click();

            string expectedProvince = "San José";
            IReadOnlyCollection<IWebElement> productElements = driver.FindElements(By.CssSelector("tr[data-province='" + expectedProvince + "']"));

            Assert.IsTrue(productElements.Count > 0, $"No se encontraron productos relacionados con la provincia {expectedProvince} después de aplicar el filtro");
        }

        // Test Funcional: Sebastián Rodríguez Tencio. Sprint 3
        [Test]
        public void ClearFiltersButton_Test()
        {
            homePage.NavigateTo();
            searchPage.Search("Apple");
            driver.FindElement(By.CssSelector("div:nth-child(8) > .province-checkbox")).Click();

            string expectedProvince = "San José";
            IReadOnlyCollection<IWebElement> productElements = driver.FindElements(By.CssSelector("tr[data-province='" + expectedProvince + "']"));
            driver.FindElement(By.Id("clear-filters")).Click();
            IReadOnlyCollection<IWebElement> provinceCheckboxes = driver.FindElements(By.CssSelector(".province-checkbox:checked"));

            Assert.That(0, Is.EqualTo(provinceCheckboxes.Count), "Los checkboxes de provincia no se desmarcaron correctamente");
        }
    }
    public class FilterHomePage
    {
        private readonly IWebDriver driver;

        public FilterHomePage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void NavigateTo()
        {
            driver.Navigate().GoToUrl("http://localhost:5064/");
            driver.Manage().Window.Size = new System.Drawing.Size(968, 1079);
        }
    }

    public class FilterSearchPage
    {
        private readonly IWebDriver driver;

        public FilterSearchPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void Search(string searchString)
        {
            driver.FindElement(By.Id("SearchString")).Click();
            driver.FindElement(By.Id("SearchString")).SendKeys(searchString);
            driver.FindElement(By.Id("searchButton")).Click();
        }
    }
}