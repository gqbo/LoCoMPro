using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace functional_tests
{
    public class FunctionalTest_Search
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

        // Test Funcional: Sebastián Rodríguez Tencio.
        [Test]
        public void SearchFunctionalTest()
        {
            driver.Navigate().GoToUrl("http://localhost:5064/");
            driver.FindElement(By.Id("SearchString")).Click();
            driver.FindElement(By.Id("SearchString")).SendKeys("Apple");
            driver.FindElement(By.Id("searchButton")).Click();
            string currentUrl = driver.Url;

            string expectedUrl = "http://localhost:5064/Records?SearchString=Apple&SearchProvince=&SearchCanton=&SearchCategory=";

            Assert.That(currentUrl, Is.EqualTo(expectedUrl));
        }
    }
}