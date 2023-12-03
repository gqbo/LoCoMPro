using LoCoMPro_LV.Models;
using LoCoMPro_LV.Data;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Pages.Records;
using LoCoMPro_LV.Pages.Reports;
using static LoCoMPro_LV.Pages.Reports.AnomaliesModel;

namespace n_unit_tests
{
    [TestFixture]
    public class UnitTests_AnomaliesDate
    {
        // Test by James Araya - B70528
        [Test]
        public async Task AnomaliesDate_LessThanThreshold_NoAnomaliesDetected()
        {
            var dbContext = ContextTest();
            var model = new AnomaliesModel(dbContext);
            var groupedRecords = CreateSampleGroupedRecords(7);
            var recordsToTest = groupedRecords.SelectMany(group => group).ToList();
            await model.AnomaliesDate(recordsToTest);
            Assert.IsFalse(dbContext.Anomalies.Any());
        }

        // Test by James Araya - B70528
        [Test]
        public async Task AnomaliesDate_LessThanThreshold_AnomaliesDetected()
        {
            var dbContext = ContextTest();
            var model = new AnomaliesModel(dbContext);
            var groupedRecords = CreateSampleGroupedRecords(8);
            var recordsToTest = groupedRecords.SelectMany(group => group).ToList();
            await model.AnomaliesDate(recordsToTest);
            Assert.IsTrue(dbContext.Anomalies.Any());
        }

        // Test by James Araya - B70528
        [Test]
        public async Task ProcessGroupedRecordsDate_LessThanThreshold_NoAnomaliesDetected()
        {
            var dbContext = ContextTest();
            var model = new AnomaliesModel(dbContext);
            var groupedRecords = CreateSampleGroupedRecords(5);
            await model.ProcessGroupedRecordsDate(groupedRecords);
            Assert.IsFalse(dbContext.Anomalies.Any());
        }

        // Test by James Araya - B70528
        [Test]
        public async Task ProcessGroupedRecords_ValidRecords_Success()
        {
            var dbContext = ContextTest();
            var model = new AnomaliesModel(dbContext);
            var groupedRecords = CreateSampleGroupedRecords(8);
            await model.ProcessGroupedRecordsDate(groupedRecords);
            Assert.IsTrue(dbContext.Anomalies.Any()); 
        }

        // Test by James Araya - B70528
        [Test]
        public async Task heuristicDateSorted_ValidRecords_Success()
        {
            var dbContext = ContextTest();
            var model = new AnomaliesModel(dbContext);
            var groupedRecords = CreateSampleGroupedRecords(8);
            var recordsToTest = groupedRecords.SelectMany(group => group).ToList();
            model.heuristicDate(recordsToTest, out var sortedRecords, out var referenceDate);

            Assert.IsTrue(sortedRecords.Any());
        }

        // Test by James Araya - B70528
        [Test]
        public async Task heuristicDatereferenceDate_ValidRecords_Success()
        {
            var dbContext = ContextTest();
            var model = new AnomaliesModel(dbContext);
            var groupedRecords = CreateSampleGroupedRecords(8);
            var recordsToTest = groupedRecords.SelectMany(group => group).ToList();
            model.heuristicDate(recordsToTest, out var sortedRecords, out var referenceDate);

            Assert.IsNotNull(referenceDate);
        }

        public LoComproContext ContextTest()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            var dbContext = new LoComproContext(options);
            var model = new AnomaliesModel(dbContext);

            return dbContext;
        }

        private List<IGrouping<GroupingKey, RecordStoreModel>> CreateSampleGroupedRecords(int recordsCount)
        {
            var groupedRecords = new List<IGrouping<GroupingKey, RecordStoreModel>>();

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
                NameProduct = "Apple Iphone 11"
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

            var record1 = new Record
            {
                NameGenerator = "anne",
                GeneratorUser = generatorUser,
                RecordDate = DateTime.Parse("2010-01-18"),
                Price = 800,
                NameStore = "Ishop",
                Latitude = 9.9516,
                Longitude = -84.0990,
                NameProduct = "Apple Iphone 11",
                Store = store,
                Product = product
            };

            var record2 = new Record
            {
                NameGenerator = "anne",
                GeneratorUser = generatorUser,
                RecordDate = DateTime.Parse("2022-01-18"),
                Price = 283000,
                NameStore = "Ishop",
                Latitude = 9.9516,
                Longitude = -84.0990,
                NameProduct = "Apple Iphone 11",
                Store = store,
                Product = product
            };

            var record3 = new Record
            {
                NameGenerator = "anne",
                GeneratorUser = generatorUser,
                RecordDate = DateTime.Parse("2022-02-18"),
                Price = 283000,
                NameStore = "Ishop",
                Latitude = 9.9516,
                Longitude = -84.0990,
                NameProduct = "Apple Iphone 11",
                Store = store,
                Product = product
            };

            var record4 = new Record
            {
                NameGenerator = "anne",
                GeneratorUser = generatorUser,
                RecordDate = DateTime.Parse("2023-01-18"),
                Price = 283000,
                NameStore = "Ishop",
                Latitude = 9.9516,
                Longitude = -84.0990,
                NameProduct = "Apple Iphone 11",
                Store = store,
                Product = product
            };

            var record5 = new Record
            {
                NameGenerator = "anne",
                GeneratorUser = generatorUser,
                RecordDate = DateTime.Parse("2023-05-18"),
                Price = 283000,
                NameStore = "Ishop",
                Latitude = 9.9516,
                Longitude = -84.0990,
                NameProduct = "Apple Iphone 11",
                Store = store,
                Product = product
            };

            if (recordsCount == 8)
            {
                var groupKey1 = new GroupingKey { NameProduct = "Apple Iphone 11", NameStore = "Ishop", NameCanton = "Tibás", NameProvince = "San José" };
                var recordToGroup1 = new RecordStoreModel { Record = record1, Store = store };
                var recordToGroup2 = new RecordStoreModel { Record = record2, Store = store };
                var recordToGroup3 = new RecordStoreModel { Record = record2, Store = store };
                var recordToGroup4 = new RecordStoreModel { Record = record3, Store = store };
                var recordToGroup5 = new RecordStoreModel { Record = record4, Store = store };
                var recordToGroup6 = new RecordStoreModel { Record = record5, Store = store };
                var recordToGroup7 = new RecordStoreModel { Record = record5, Store = store };
                var recordToGroup8 = new RecordStoreModel { Record = record4, Store = store };

                var group1 = new List<RecordStoreModel> { recordToGroup1, recordToGroup2, recordToGroup3, recordToGroup4, recordToGroup5, recordToGroup6, recordToGroup7, recordToGroup8 };
                var grouping1 = group1.GroupBy(r => groupKey1);

                groupedRecords.AddRange(grouping1);
            }
            else if(recordsCount == 7)
            {
                // Crear algunos datos simulados para usar en la lista agrupada
                var groupKey1 = new GroupingKey { NameProduct = "Apple Iphone 11", NameStore = "Ishop", NameCanton = "Tibás", NameProvince = "San José" };
                var recordToGroup1 = new RecordStoreModel { Record = record2, Store = store };
                var recordToGroup2 = new RecordStoreModel { Record = record2, Store = store };
                var recordToGroup3 = new RecordStoreModel { Record = record2, Store = store };
                var recordToGroup4 = new RecordStoreModel { Record = record2, Store = store };
                var recordToGroup5 = new RecordStoreModel { Record = record2, Store = store };
                var recordToGroup6 = new RecordStoreModel { Record = record2, Store = store };
                var recordToGroup7 = new RecordStoreModel { Record = record2, Store = store };

                var group1 = new List<RecordStoreModel> { recordToGroup1, recordToGroup2, recordToGroup3, recordToGroup4, recordToGroup5, recordToGroup6, recordToGroup7};
                var grouping1 = group1.GroupBy(r => groupKey1);

                groupedRecords.AddRange(grouping1);
            }
            else if (recordsCount == 5)
            {
                var groupKey1 = new GroupingKey { NameProduct = "Apple Iphone 11", NameStore = "Ishop", NameCanton = "Tibás", NameProvince = "San José" };
                var recordToGroup1 = new RecordStoreModel { Record = record2, Store = store };
                var recordToGroup2 = new RecordStoreModel { Record = record3, Store = store };
                var recordToGroup3 = new RecordStoreModel { Record = record3, Store = store };
                var recordToGroup4 = new RecordStoreModel { Record = record2, Store = store };
                var recordToGroup5 = new RecordStoreModel { Record = record3, Store = store };

                var group1 = new List<RecordStoreModel> { recordToGroup1, recordToGroup2, recordToGroup3, recordToGroup4, recordToGroup5 };
                var grouping1 = group1.GroupBy(r => groupKey1);

                groupedRecords.AddRange(grouping1);
            }

            return groupedRecords;
        }
    }
}
