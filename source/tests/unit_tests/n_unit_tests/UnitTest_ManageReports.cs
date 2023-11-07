
using LoCoMPro_LV.Models;
using LoCoMPro_LV.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using LoCoMPro_LV.Utils;
using LoCoMPro_LV.Pages.Reports;
using Microsoft.AspNetCore.Mvc;

namespace n_unit_tests
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

            Assert.IsInstanceOf<RedirectToPageResult>(result);
            var redirectToPageResult = (RedirectToPageResult)result;
            Assert.That(redirectToPageResult.PageName, Is.EqualTo("./Index"));
        }

        // Test by Cristopher Hernandez Calderon - C13632
        [Test]
        public async Task OnPostAsync_InvalidFormat()
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

            var result = await model.OnPostAsync("accept", "Crish", DateTime.Parse("2022-01-20"));

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        // Test by Cristopher Hernandez Calderon - C13632
        [Test]
        public async Task OnPostAsync_ValidFormat_SetAccept()
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
        }

        // Test by Cristopher Hernandez Calderon - C13632
        [Test]
        public async Task GetReportsForRecordAsync_ReportsFound_ReturnsReportsList()
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
            var report2 = CreateSampleReport2();

            dbContext.Add(report);
            dbContext.Add(report2);
            dbContext.SaveChanges();

            var model = new DetailsModel(dbContext, databaseUtils);

            var result = await model.GetReportsForRecordAsync("anne", DateTime.Parse("2022-1-18"));
          
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<Report>>(result);
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        // Test by Cristopher Hernandez Calderon - C13632
        [Test]
        public async Task GetReportsForRecordAsync_NoReportsFound_ReturnsEmptyList()
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
            var report2 = CreateSampleReport2();

            dbContext.Add(report);
            dbContext.Add(report2);
            dbContext.SaveChanges();

            var model = new DetailsModel(dbContext, databaseUtils);

            var result = await model.GetReportsForRecordAsync("brad", DateTime.Parse("2022-1-18"));

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<Report>>(result);
            Assert.That(result.Count(), Is.EqualTo(0));
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

        private Report CreateSampleReport2()
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
                NameReporter = "brad",
                ReportDate = DateTime.Parse("2022-02-20"),
            };
        }
    }
}