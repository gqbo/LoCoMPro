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

        public void SearchForProduct(string searchString)
        {
            _driver.FindElement(By.Id("SearchString")).Click();
            _driver.FindElement(By.Id("SearchString")).SendKeys(searchString);
            _driver.FindElement(By.Id("searchButton")).Click();
        }

        public void ClickOnProductLink(string productLinkText)
        {
            _driver.FindElement(By.LinkText(productLinkText)).Click();
        }
    }

    public class ListPage
    {
        private readonly IWebDriver _driver;

        public ListPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public void AddOrDeleteToList()
        {
            _driver.FindElement(By.CssSelector(".button__icon")).Click();
        }

        public void ClickMyListDropdown()
        {
            _driver.FindElement(By.Id("trigger")).Click();
            _driver.FindElement(By.LinkText("Mi lista de interés")).Click();
        }

        public void FindStoresWithList()
        {
            _driver.FindElement(By.LinkText("Ubicar tiendas")).Click();
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
            searchResultsPage.SearchForProduct("Apple");
            searchResultsPage.ClickOnProductLink("Apple Iphone 11 64gb");

            ListPage listPage = new ListPage(driver);
            listPage.AddOrDeleteToList();

            Assert.IsTrue(driver.PageSource.Contains("Quitar de la lista"));
        }

        // Test Funcional:  Gabriel González Flores. Sprint 3
        [Test]
        public void CheckMyList()
        {
            LoginPage loginPage = new LoginPage(driver);
            loginPage.LoginUser("gabriel", "Gabriel1.");

            ListPage listPage = new ListPage(driver);
            listPage.ClickMyListDropdown();

            Assert.IsTrue(driver.PageSource.Contains("Mi Lista"));
        }

        // Test Funcional:  Gabriel González Flores. Sprint 3
        [Test]
        public void FindStoresWithList()
        {
            LoginPage loginPage = new LoginPage(driver);
            loginPage.LoginUser("gabriel", "Gabriel1.");

            SearchResultsPage searchResultsPage = new SearchResultsPage(driver);
            searchResultsPage.SearchForProduct("Apple");
            searchResultsPage.ClickOnProductLink("Apple Iphone 11 64gb");

            ListPage listPage = new ListPage(driver);
            listPage.ClickMyListDropdown();
            listPage.FindStoresWithList();

            Assert.IsTrue(driver.PageSource.Contains("Búsqueda de mi lista"));

            listPage.ClickMyListDropdown();
        }
    }
}
