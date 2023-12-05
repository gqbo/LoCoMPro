using LoCoMPro_LV.Models;
using LoCoMPro_LV.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using LoCoMPro_LV.Utils;
using LoCoMPro_LV.Pages.Lists ;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace n_unit_tests
{
    [TestFixture]
    public class UnitTests_ResultList
    {
        // Test by Gabriel González Flores - C03376. Sprint 3
        [Test]
        public async Task GetFirstRecordAsync_Valid()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var dbContext = new LoComproContext(options);

            var record = CreateSampleRecord();

            dbContext.Add(record);
            dbContext.SaveChanges();

            var model = new ResultListModel(dbContext, userManager: null);

            model.NameGenerator = "anne";
            model.RecordDate = DateTime.Parse("2022-01-18");

            var actualRecord = await model.GetFirstRecordAsync();

            Assert.IsNotNull(actualRecord);
        }

        // Test by Gabriel González Flores - C03376. Sprint 3
        [Test]
        public async Task GetFirstRecordAsync_Invalid()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var dbContext = new LoComproContext(options);

            var model = new ResultListModel(dbContext, userManager: null);

            var actualRecord = await model.GetFirstRecordAsync();

            Assert.IsNull(actualRecord);
        }

        // Test by Gabriel González Flores - C03376. Sprint 3
        [Test]
        public async Task GetStoreForRecordAsync_Valid()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var dbContext = new LoComproContext(options);

            var record = CreateSampleRecord();
            var store = CreateSampleStore();

            dbContext.Add(store);
            dbContext.Add(record);
            dbContext.SaveChanges();

            var model = new ResultListModel(dbContext, userManager: null);

            var actualStore = await model.GetStoreForRecordAsync(record);

            Assert.IsNotNull(actualStore);
        }

        // Test by Gabriel González Flores - C03376. Sprint 3
        [Test]
        public async Task GetStoreForRecordAsync_Invalid()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var dbContext = new LoComproContext(options);

            var record = CreateSampleRecordWithNoMatchingStore();

            dbContext.Add(record);
            dbContext.SaveChanges();

            var model = new ResultListModel(dbContext, userManager: null);

            var actualStore = await model.GetStoreForRecordAsync(record);

            Assert.IsNotNull(actualStore);
        }

        // Test by Gabriel González Flores - C03376. Sprint 3
        [Test]
        public async Task GetRecordsForStoreAsync_ValidRecord()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var dbContext = new LoComproContext(options);

            var store = CreateSampleStore();
            var record = CreateSampleRecord();
            var record2 = CreateSampleRecord2();
            var listed = CreateSampleListed();
            var listed2 = CreateSampleListed2();

            dbContext.Add(store);
            dbContext.Add(record);
            dbContext.Add(record2);
            dbContext.Add(listed);
            dbContext.Add(listed2);
            dbContext.SaveChanges();

            var model = new ResultListModel(dbContext, userManager: null);

            var tempRecord = new Record
            {
                NameStore = "Ishop",
                Latitude = 9.9516,
                Longitude = -84.0990
            };

            var result = await model.GetRecordsForStoreAsync(record);

            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));
        }

        // Test by Gabriel González Flores - C03376. Sprint 3
        [Test]
        public async Task GetRecordsForStoreAsync_InvalidRecord()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var dbContext = new LoComproContext(options);

            var store = CreateSampleStore();
            var record = CreateSampleRecord();
            var record2 = CreateSampleRecord2();
            var listed = CreateSampleListed();
            var listed2 = CreateSampleListed2();

            dbContext.Add(store);
            dbContext.Add(record);
            dbContext.Add(record2);
            dbContext.Add(listed);
            dbContext.Add(listed2);
            dbContext.SaveChanges();

            var model = new ResultListModel(dbContext, userManager: null);

            var tempRecord = new Record
            {
                NameStore = "NoMatch",
                Latitude = 9.9516,
                Longitude = -84.0990
            };

            var result = await model.GetRecordsForStoreAsync(tempRecord);

            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        private LoCoMPro_LV.Models.List CreateSampleList()
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

            return new LoCoMPro_LV.Models.List
            {
                NameList = "anne",
                UserName = "anne",
            };
        }

        private Listed CreateSampleListed()
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

            var product = new Product
            {
                NameProduct = "Apple Iphone 11 64gb"
            };

            var generatorUser = new GeneratorUser
            {
                UserName = "anne",
                ApplicationUser = user
            };

            var list = new LoCoMPro_LV.Models.List
            {
                NameList = "anne",
                UserName = "anne",
            };

            return new Listed
            {
                NameList = "anne",
                UserName = "anne",
                NameProduct = "Apple Iphone 11 64gb"

            };
        }

        private Listed CreateSampleListed2()
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

            var product = new Product
            {
                NameProduct = "Apple Iphone 13 64gb"
            };

            var generatorUser = new GeneratorUser
            {
                UserName = "anne",
                ApplicationUser = user
            };

            var list = new LoCoMPro_LV.Models.List
            {
                NameList = "anne",
                UserName = "anne",
            };

            return new Listed
            {
                NameList = "anne",
                UserName = "anne",
                NameProduct = "Apple Iphone 13 64gb"

            };
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
                Product = product
            };
        }

        private Record CreateSampleRecord2()
        {

            var product = new Product
            {
                NameProduct = "Apple Iphone 13 64gb"
            };

            return new Record
            {
                NameGenerator = "anne",
                RecordDate = DateTime.Parse("2022-01-20"),
                Price = 29000,
                NameStore = "Ishop",
                Latitude = 9.9516,
                Longitude = -84.0990,
                NameProduct = "Apple Iphone 13 64gb",
                Product = product
            };
        }

        private Record CreateSampleRecordWithNoMatchingStore()
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
                NameStore = "NoMatch",
                Canton = canton,
                NameProvince = "San José",
                NameCanton = "Tibás",
                Latitude = 9.9513,
                Longitude = -84.0991
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

        private Store CreateSampleStore()
        {
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

            return new Store
            {
                NameStore = "Ishop",
                Canton = canton,
                NameProvince = "San José",
                NameCanton = "Tibás",
                Latitude = 9.9516,
                Longitude = -84.0990
            };
        }
    }
}
