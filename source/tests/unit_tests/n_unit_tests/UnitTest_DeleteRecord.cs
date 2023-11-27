using LoCoMPro_LV.Data;
using LoCoMPro_LV.Models;
using LoCoMPro_LV.Pages.Records;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace n_unit_tests
{
    [TestFixture]
    public class UnitTest_DeleteRecord
    {
        // Test by Yordi Robles Siles - C06557. Sprint 3
        [Test]
        public async Task GetValorationsUser()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var dbContext = new LoComproContext(options);

            var mockConfiguration = new Mock<IConfiguration>();

            mockConfiguration.Setup(c => c["SomeSetting"]).Returns("TestSettingValue");

            var record = CreateSampleRecord();
            var valoration1 = CreateSampleValoration("gabriel");
            var valoration2 = CreateSampleValoration("sebastian");
            var valoration3 = CreateSampleValoration("james");
            dbContext.Add(record);
            dbContext.Add(valoration1);
            dbContext.Add(valoration2);
            dbContext.Add(valoration3);
            dbContext.SaveChanges();

            var model = new MyRecordsModel(dbContext);

            model.Username = record.NameGenerator;
            model.RecordDate = record.RecordDate;

            await dbContext.SaveChangesAsync();
            var result = await model.GetValorations(model.Username);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(3));
            Assert.That(result.First().NameGenerator, Is.EqualTo("anne"));

        }

        // Test by Yordi Robles Siles - C06557. Sprint 3
        [Test]
        public async Task GetReportsUser()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var dbContext = new LoComproContext(options);

            var mockConfiguration = new Mock<IConfiguration>();

            mockConfiguration.Setup(c => c["SomeSetting"]).Returns("TestSettingValue");

            var record = CreateSampleRecord();
            var report1 = CreateSampleReport("gabriel");
            var report2 = CreateSampleReport("sebastian");
            var report3 = CreateSampleReport("james");
            dbContext.Add(record);
            dbContext.Add(report1);
            dbContext.Add(report2);
            dbContext.Add(report3);
            dbContext.SaveChanges();

            var model = new MyRecordsModel(dbContext);

            model.Username = record.NameGenerator;
            model.RecordDate = record.RecordDate;

            await dbContext.SaveChangesAsync();
            var result = await model.GetReports(model.Username);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(3));
            Assert.That(result.First().NameGenerator, Is.EqualTo("anne"));

        }

        // Test by Yordi Robles Siles - C06557. Sprint 3
        [Test]
        public async Task DeleteReportsUser()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var dbContext = new LoComproContext(options);

            var mockConfiguration = new Mock<IConfiguration>();

            mockConfiguration.Setup(c => c["SomeSetting"]).Returns("TestSettingValue");

            var record = CreateSampleRecord();
            var report1 = CreateSampleReport("gabriel");
            var report2 = CreateSampleReport("sebastian");
            var report3 = CreateSampleReport("james");
            dbContext.Add(record);
            dbContext.Add(report1);
            dbContext.Add(report2);
            dbContext.Add(report3);
            dbContext.SaveChanges();

            var model = new MyRecordsModel(dbContext);

            model.Username = record.NameGenerator;
            model.RecordDate = record.RecordDate;

            await model.DeleteReports(record.NameGenerator);
            await dbContext.SaveChangesAsync();

            var result = dbContext.Reports.FirstOrDefault(item => item.NameGenerator == record.NameGenerator && item.RecordDate == record.RecordDate);
            Assert.That(result, Is.Null);
        }

        // Test by Yordi Robles Siles - C06557. Sprint 3
        [Test]
        public async Task DeleteValorationsUser()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var dbContext = new LoComproContext(options);

            var mockConfiguration = new Mock<IConfiguration>();

            mockConfiguration.Setup(c => c["SomeSetting"]).Returns("TestSettingValue");

            var record = CreateSampleRecord();
            var valoration1 = CreateSampleValoration("gabriel");
            var valoration2 = CreateSampleValoration("sebastian");
            var valoration3 = CreateSampleValoration("james");
            dbContext.Add(record);
            dbContext.Add(valoration1);
            dbContext.Add(valoration2);
            dbContext.Add(valoration3);
            dbContext.SaveChanges();

            var model = new MyRecordsModel(dbContext);

            model.Username = record.NameGenerator;
            model.RecordDate = record.RecordDate;

            await model.DeleteValorations(record.NameGenerator);
            await dbContext.SaveChangesAsync();

            var result = dbContext.Valorations.FirstOrDefault(item => item.NameGenerator == record.NameGenerator && item.RecordDate == record.RecordDate);
            Assert.That(result, Is.Null);
        }


        // Test by Yordi Robles Siles - C06557. Sprint 3
        [Test]
        public async Task DeleteRecordUser()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var dbContext = new LoComproContext(options);

            var mockConfiguration = new Mock<IConfiguration>();

            mockConfiguration.Setup(c => c["SomeSetting"]).Returns("TestSettingValue");

            var record = CreateSampleRecord();

            var valoration1 = CreateSampleValoration("gabriel");
            var valoration2 = CreateSampleValoration("sebastian");
            var valoration3 = CreateSampleValoration("james");

            var report1 = CreateSampleReport("gabriel");
            var report2 = CreateSampleReport("sebastian");
            var report3 = CreateSampleReport("james");

            dbContext.Add(record);

            dbContext.Add(valoration1);
            dbContext.Add(valoration2);
            dbContext.Add(valoration3);

            dbContext.Add(report1);
            dbContext.Add(report2);
            dbContext.Add(report3);
            dbContext.SaveChanges();

            var model = new MyRecordsModel(dbContext);

            model.Username = record.NameGenerator;
            model.RecordDate = record.RecordDate;

            await model.OnPostDeleteRecord();
            await dbContext.SaveChangesAsync();

            var result = dbContext.Records.FirstOrDefault(item => item.NameGenerator == record.NameGenerator && item.RecordDate == record.RecordDate);
            Assert.That(result, Is.Null);
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
                Price = 280000,
                NameStore = "Ishop",
                Latitude = 9.9516,
                Longitude = -84.0990,
                NameProduct = "Apple Iphone 11 64gb",
                Store = store,
                Product = product
            };
        }
        private Evaluate CreateSampleValoration(string nameEvaluator)
        {
            return new Evaluate
            {
                NameEvaluator = nameEvaluator,
                NameGenerator = "anne",
                RecordDate = DateTime.Parse("2022-01-18"),
                StarsCount = 3
            };
        }

        private Report CreateSampleReport(string nameReporter)
        {
            return new Report
            {
                NameReporter = nameReporter,
                NameGenerator = "anne",
                RecordDate = DateTime.Parse("2022-01-18"),
                ReportDate = DateTime.Parse("2023-01-10"),
                Comment = "Test comment",
                State = 0
            };
        }
    }
}
