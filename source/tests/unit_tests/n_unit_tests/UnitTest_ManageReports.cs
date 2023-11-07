﻿
using LoCoMPro_LV.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LoCoMPro_LV.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using LoCoMPro_LV.Areas.Identity.Pages.Account;
using LoCoMPro_LV.Utils;
using LoCoMPro_LV.Pages.Reports;
using Microsoft.AspNetCore.Mvc;

namespace nunit_tests
{
    [TestFixture]
    public class UnitTest_ManageReports
    {
        // Test by Cristopher Hernandez Calderon - C13632
        [Test]
        public async Task OnPostAsync_ValidFormat()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var dbContext = new LoComproContext(options);

            var mockConfiguration = new Mock<IConfiguration>();

            mockConfiguration.Setup(c => c["SomeSetting"]).Returns("TestSettingValue");

            var databaseUtils = new DatabaseUtils(mockConfiguration.Object);

            var report = CreateSampleReport();

            dbContext.Add(report);
            dbContext.SaveChanges();

            var model = new DetailsModel(dbContext, databaseUtils);

            model.NameGenerator = "anne";
            model.RecordDate = DateTime.Parse("2022-1-18");

            var result = await model.OnPostAsync("accept", "Crish", DateTime.Parse("2022-01-20"));

            Assert.That(report.State, Is.EqualTo(1));
            Assert.IsInstanceOf<RedirectToPageResult>(result);
            var redirectToPageResult = (RedirectToPageResult)result;
            Assert.That(redirectToPageResult.PageName, Is.EqualTo("./Index"));
        }

        // Test by Cristopher Hernandez Calderon - C13632
        [Test]
        public async Task OnPostAsync_InvalidForma_SetReject()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var dbContext = new LoComproContext(options);

            var mockConfiguration = new Mock<IConfiguration>();

            mockConfiguration.Setup(c => c["SomeSetting"]).Returns("TestSettingValue");

            var databaseUtils = new DatabaseUtils(mockConfiguration.Object);

            var report = CreateSampleReport();

            dbContext.Add(report);
            dbContext.SaveChanges();

            var model = new DetailsModel(dbContext, databaseUtils);

            model.NameGenerator = "anne";
            model.RecordDate = DateTime.Parse("2022-1-18");

            var result = await model.OnPostAsync("reject", "Crish", DateTime.Parse("2022-01-20"));

            Assert.That(report.State, Is.EqualTo(2));
            Assert.IsInstanceOf<RedirectToPageResult>(result);
            var redirectToPageResult = (RedirectToPageResult)result;
            Assert.That(redirectToPageResult.PageName, Is.EqualTo("./Index"));
        }

        private Report CreateSampleReport()
        {
            var user = new ApplicationUser
            {
                UserName = "anne",
                FirstName = "Anne",
                LastName = "Hathaway",
                Latitude = 10.009,
                Longitude = -84.1211,
                NameProvince = "Heredia",
                NameCanton = "Barva",
                NormalizedUserName = "ANNE",
                Email = "anne@gmail.com",
                NormalizedEmail = "ANNE@GMAIL.COM",
                EmailConfirmed = false,
                PasswordHash = "AQAAAAIAAYagAAAAEJmsnmp+Rm7C6VMHu1s21eBFFButNJUpJ6E6yV5OnERn6c2Hv7KutoQrAaUPyez1lQ==",
                SecurityStamp = "5NFRN2JU2Z7T7WPCJZ4LY4TA45YHLSXW",
                ConcurrencyStamp = "cd77e5d0-740a-43f8-9ad4-3d8633ec6d01",
                LockoutEnabled = true,
                AccessFailedCount = 0
            };

            var generatorUser = new GeneratorUser
            {
                UserName = "anne",
                ApplicationUser = user
            };

            var product = new Product
            {
                NameProduct = "Apple Iphone 11 64gb"
            };

            var province = new Province
            {
                NameProvince = "San José"
            };

            var canton = new Canton
            {
                NameCanton = "Tibás",
                Province = province,
                NameProvince = "San José"
            };

            var store = new Store
            {
                NameStore = "Ishop",
                Canton = canton,
                NameProvince = "San José",
                NameCanton = "Tibás",
                Latitude = 9.9516,
                Longitude = -84.0990
            };

            var record = new Record
            {
                NameGenerator = "anne",
                GeneratorUser = generatorUser,
                RecordDate = DateTime.Parse("2022-01-18"),
                Price = 280000.5643,
                NameStore = "Ishop",
                Latitude = 9.9516,
                Longitude = -84.0990,
                NameProduct = "Apple Iphone 11 64gb",
                Store = store,
                Product = product
            };

            return new Report
            {
                NameGenerator = "anne",
                RecordDate = DateTime.Parse("2022-01-18"),
                NameReporter = "Crish",
                ReportDate = DateTime.Parse("2022-01-20"),
            };
        }
    }
}