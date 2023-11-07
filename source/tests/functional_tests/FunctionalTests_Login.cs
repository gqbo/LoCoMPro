using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;

namespace functional_tests
{

    public class FunctionalTest_Login
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

        // Test Funcional: Gabriel González Flores. Sprint 2
        [Test]
        public void LoginFunctionalTest()
        {
            driver.Navigate().GoToUrl("http://localhost:5064/");
            driver.FindElement(By.LinkText("Iniciar Sesión")).Click();
            driver.FindElement(By.Id("Input_UserName")).Click();
            driver.FindElement(By.Id("Input_UserName")).SendKeys("gabriel");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("Gabriel1.");
            driver.FindElement(By.CssSelector(".register_submit")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(5000);
            string currentUrl = driver.Url;

            string expectedUrl = "http://localhost:5064/";

            Assert.That(currentUrl, Is.EqualTo(expectedUrl));
        }
    }
}