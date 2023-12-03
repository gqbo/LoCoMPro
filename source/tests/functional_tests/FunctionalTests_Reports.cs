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


        // Test Functional: Yordi Robles Siles. Sprint 3
        [Test]
        public void TopReportsFunctionalTest()
        {
            LoginPage loginPage = new LoginPage(driver);
            loginPage.Login("Admin", "Admin1.");
            TopReportsPage topReportsPage = new TopReportsPage(driver);
            topReportsPage.SearchTopReports();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(5000);
            string currentUrl = driver.Url;

            string expectedUrl = "http://localhost:5064/Reports/TopReports";

            Assert.That(currentUrl, Is.EqualTo(expectedUrl));
        }
    }
    public class LoginPage
    {
        private IWebDriver driver;
        public LoginPage(IWebDriver driver) {
            this.driver = driver;
        }

        public void Login(string Username, string Password)
        {
            driver.Navigate().GoToUrl("http://localhost:5064/");
            driver.FindElement(By.LinkText("Iniciar Sesión")).Click();
            driver.FindElement(By.Id("Input_UserName")).Click();
            driver.FindElement(By.Id("Input_UserName")).SendKeys(Username);
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys(Password);
            driver.FindElement(By.CssSelector(".register_submit")).Click();
        }
    }

    public class TopReportsPage
    {
        private IWebDriver driver;
        public TopReportsPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void SearchTopReports()
        {
            driver.FindElement(By.Id("trigger1")).Click();
            driver.FindElement(By.LinkText("Top Reportados")).Click();
        }
    }
}