using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;


namespace functional_tests
{
    // Clase de la página de resultados de búsqueda
    public class SearchResultsPage
    {
        private readonly IWebDriver _driver;

        public SearchResultsPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public void SearchForProduct(string searchString, string productLinkText)
        {
            _driver.FindElement(By.Id("SearchString")).Click();
            _driver.FindElement(By.Id("SearchString")).SendKeys(searchString);
            _driver.FindElement(By.Id("searchButton")).Click();
            _driver.FindElement(By.LinkText(productLinkText)).Click();
            _driver.FindElement(By.CssSelector(".button__icon")).Click();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(20000);
        }
    }

    public class FunctionalTests_AddtoList
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

        // Test Funcional:  Cristopher Hernandez Calderon. Sprint 3
        [Test]
        public void AddProductToList()
        {
            LoginPage loginPage = new LoginPage(driver);
            loginPage.LoginUser("yordi", "Yordi1.");

            SearchResultsPage searchResultsPage = new SearchResultsPage(driver);
            searchResultsPage.SearchForProduct("Apple", "Apple Iphone 11 64gb");

            Assert.IsTrue(driver.PageSource.Contains("Quitar de la lista"));
        }
    }
}
