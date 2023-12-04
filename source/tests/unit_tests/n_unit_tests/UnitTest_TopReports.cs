using LoCoMPro_LV.Data;
using LoCoMPro_LV.Models;
using LoCoMPro_LV.Pages.Reports;
using LoCoMPro_LV.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace n_unit_tests
{
    public class UnitTest_TopReports
    {
        // Test by Yordi Robles Siles - C06557. Sprint 3
        [Test]
        public async Task GetAcceptedReportsCountTestAsync()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var dbContext = new LoComproContext(options);
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["SomeSetting"]).Returns("TestSettingValue");
            var databaseUtils = new DatabaseUtils(mockConfiguration.Object);

            Record record = CreateSampleRecord();
            Report report1 = CreateSampleReport("yordi", "2023-01-18", 1);
            Report report2 = CreateSampleReport("Sebastian", "2020-01-18", 1);

            dbContext.Add(record);
            dbContext.SaveChanges();
            dbContext.Add(report1);
            dbContext.Add(report2);
            dbContext.SaveChanges();
            var model = new TopReportsModel(dbContext, databaseUtils);
            var count = await model.GetAcceptedReportsCount("anne");

            Assert.That(count, Is.EqualTo(2));
        }

        // Test by Yordi Robles Siles - C06557. Sprint 3
        [Test]
        public async Task GetAcceptedReportsNotFoundTest()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var dbContext = new LoComproContext(options);
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["SomeSetting"]).Returns("TestSettingValue");
            var databaseUtils = new DatabaseUtils(mockConfiguration.Object);

            Record record = CreateSampleRecord();
            Report report1 = CreateSampleReport("yordi", "2023-01-18", 0);
            Report report2 = CreateSampleReport("Sebastian", "2020-01-18", 2);

            dbContext.Add(record);
            dbContext.SaveChanges();
            dbContext.Add(report1);
            dbContext.Add(report2);
            dbContext.SaveChanges();
            var model = new TopReportsModel(dbContext, databaseUtils);
            var count = await model.GetAcceptedReportsCount("anne");

            Assert.That(count, Is.EqualTo(0));
        }

        private Report CreateSampleReport(string NameReporter, string ReportDate, int state)
        {
            var report = new Report
            {
                NameGenerator = "anne",
                RecordDate = DateTime.Parse("2022-01-18"),
                NameReporter = NameReporter,
                ReportDate = DateTime.Parse(ReportDate),
                State = state
            };
            return report;
        }

        private Record CreateSampleRecord()
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

            return new Record
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
        }
    }
}