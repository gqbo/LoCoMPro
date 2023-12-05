using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;


namespace functional_tests
{
        public class LoginPage
        {
            private IWebDriver driver;

            public LoginPage(IWebDriver driver)
            {
                this.driver = driver;
            }

            public void LoginUser(string userName, string password)
            {
                driver.Navigate().GoToUrl("http://localhost:5064/");
                driver.FindElement(By.LinkText("Iniciar Sesión")).Click();
                driver.FindElement(By.Id("Input_UserName")).SendKeys(userName);
                driver.FindElement(By.Id("Input_Password")).SendKeys(password);
                driver.FindElement(By.CssSelector(".row:nth-child(2) > .col")).Click();
                driver.FindElement(By.CssSelector(".register_submit")).Click();
            }
        }

        public class AddRecordPage
        {
            private IWebDriver driver;

            public AddRecordPage(IWebDriver driver)
            {
                this.driver = driver;
            }

            public void CreateStore()
            {
                driver.FindElement(By.Id("AgregarProducto")).Click();
                var element = driver.FindElement(By.Id("map"));
                Actions builder = new Actions(driver);
                builder.MoveToElement(element).ClickAndHold().Perform();
                builder.MoveToElement(element).Perform();
                builder.MoveToElement(element).Release().Perform();
                driver.FindElement(By.Id("map")).Click();
                var markerElement = driver.FindElement(By.CssSelector(".leaflet-marker-icon"));
                builder.MoveToElement(markerElement).ClickAndHold().Perform();
                builder.MoveToElement(markerElement).Perform();
                builder.MoveToElement(markerElement).Release().Perform();
                driver.FindElement(By.CssSelector(".leaflet-marker-icon")).Click();
            }

            public void CreateRecord(string store, string product, string price, string description, string category)
            {
                driver.FindElement(By.Id("Store")).Click();
                driver.FindElement(By.Id("Store")).SendKeys(store);
                driver.FindElement(By.CssSelector(".add_confirm")).Click();
                driver.FindElement(By.Id("Products")).Click();
                driver.FindElement(By.Id("Products")).SendKeys(product);
                driver.FindElement(By.Id("precioInput")).Click();
                driver.FindElement(By.Id("precioInput")).SendKeys(price);
                driver.FindElement(By.Id("Record_Description")).Click();
                driver.FindElement(By.Id("Record_Description")).SendKeys(description);
                driver.FindElement(By.Id("Category")).Click();
                var dropdown = driver.FindElement(By.Id("Category"));
                dropdown.FindElement(By.XPath($"//option[. = '{category}']")).Click();
                driver.FindElement(By.CssSelector(".add_confirm")).Click();
            }
        }

        public class MyRecordsPage
        {
            private IWebDriver driver;

            public MyRecordsPage(IWebDriver driver)
            {
                this.driver = driver;
            }

            public void DeleteRecord()
            {
                driver.FindElement(By.Id("trigger")).Click();
                driver.FindElement(By.LinkText("Mis aportes")).Click();
                driver.FindElement(By.CssSelector("tr:nth-child(1) .button_delete")).Click();
        }
        }

        public class FunctionalTests_DeleteRecord
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

            // Test Funcional:  Yordi Robles Siles. Sprint 3
            [Test]
            public void CreateRecordTest()
            {
                LoginPage loginPage = new LoginPage(driver);
                loginPage.LoginUser("yordi", "Yordi1.");

                AddRecordPage addRecordPage = new AddRecordPage(driver);
                addRecordPage.CreateStore();
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(50000);
                addRecordPage.CreateRecord("Pali", "Esto es una prueba", "1000", "Esto es una prueba.", "Moda");

                string currentUrl = driver.Url;
                string expectedUrl = "http://localhost:5064/";
                Assert.That(currentUrl, Is.EqualTo(expectedUrl));
            }

            // Test Funcional:  Yordi Robles Siles. Sprint 3
            [Test]
            public void DeleteRecordTest()
            {
                LoginPage loginPage = new LoginPage(driver);
                loginPage.LoginUser("yordi", "Yordi1.");
                MyRecordsPage myRecordsPage = new MyRecordsPage(driver);
                myRecordsPage.DeleteRecord();
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(50000);

                string currentUrl = driver.Url;
                string expectedUrl = "http://localhost:5064/Records/MyRecords";
                Assert.That(currentUrl, Is.EqualTo(expectedUrl));
            }
    }
}
