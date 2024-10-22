﻿using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace functional_tests
{
    public class FunctionalTest_AdvancedSearch
    {
        IWebDriver driver;
        private SearchPage searchPage;
        private HomePage homePage;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            homePage = new HomePage(driver);
            searchPage = new SearchPage(driver);
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
            homePage.NavigateTo();
            driver.FindElement(By.CssSelector(".advanced_search")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(50000);
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
            searchPage.Search("Apple");

            Assert.IsTrue(driver.PageSource.Contains("Apple") &&
              driver.PageSource.Contains("San José") &&
              driver.PageSource.Contains("Tibás") &&
              driver.PageSource.Contains("Celulares"));
        }
    }

    public class HomePage
    {
        private readonly IWebDriver driver;

        public HomePage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void NavigateTo()
        {
            driver.Navigate().GoToUrl("http://localhost:5064/");
            driver.Manage().Window.Size = new System.Drawing.Size(968, 1079);
        }
    }

    public class SearchPage
    {
        private readonly IWebDriver driver;

        public SearchPage(IWebDriver driver)
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