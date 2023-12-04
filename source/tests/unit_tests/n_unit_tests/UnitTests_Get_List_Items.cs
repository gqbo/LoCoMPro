using LoCoMPro_LV.Models;
using LoCoMPro_LV.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using LoCoMPro_LV.Utils;
using LoCoMPro_LV.Pages.Lists ;
using Microsoft.AspNetCore.Identity;

namespace n_unit_tests
{
    [TestFixture]
    public class UnitTests_Get_List_Items
    {
        // Test by Cristopher Hernandez Calderon - C13632. Sprint 3
        [Test]
        public async Task GetListItems()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var dbContext = new LoComproContext(options);

            var mockConfiguration = new Mock<IConfiguration>();

            mockConfiguration.Setup(c => c["SomeSetting"]).Returns("TestSettingValue");

            var record = CreateSampleRecord();
            var list = CreateSampleList();
            var listed = CreateSampleListed();
            var listed2 = CreateSampleListed2();

            dbContext.Add(record);
            dbContext.Add(list);
            dbContext.Add(listed);
            dbContext.Add(listed2);
            dbContext.SaveChanges(); ;

            var model = new IndexModel(dbContext);

            var items = await model.GetListedItemsAsync("anne");

            List<Listed> expected_items = new List<Listed>
            {
                listed,
                listed2
            };

            CollectionAssert.AreEqual(expected_items, items);
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
