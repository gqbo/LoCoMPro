using LoCoMPro_LV.Models;
using LoCoMPro_LV.Data;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Pages.Records;
using LoCoMPro_LV.Pages.Reports;
using static LoCoMPro_LV.Pages.Reports.AnomaliesModel;

namespace n_unit_tests
{
    [TestFixture]
    public class UnitTests_AnomaliesModel
    {
        // Test by Sebasti�n Rodr�guez Tencio - C06756. Sprint 3
        [Test]
        public async Task ProcessGroupedRecordsPrice_LessThanThreshold_NoAnomaliesDetected()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            var dbContext = new LoComproContext(options);
            var model = new AnomaliesModel(dbContext);
            var groupedRecords = CreateSampleGroupedRecords(5);

            await model.ProcessGroupedRecordsPrice(groupedRecords);

            Assert.IsFalse(dbContext.Anomalies.Any()); // Verifica que no haya anomal�as en la base de datos
        }

        // Test by Sebasti�n Rodr�guez Tencio - C06756. Sprint 3
        [Test]
        public async Task ProcessGroupedRecordsPrice_ValidRecords_Success()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            var dbContext = new LoComproContext(options);
            var model = new AnomaliesModel(dbContext);
            var groupedRecords = CreateSampleGroupedRecords(8);

            await model.ProcessGroupedRecordsPrice(groupedRecords);

            Assert.IsTrue(dbContext.Anomalies.Any()); // Verifica que haya al menos una anomal�a agregada
        }

        // Test by Sebasti�n Rodr�guez Tencio - C06756. Sprint 3
        [Test]
        public void CalculateQ2Index_EvenCount_ReturnsMiddleIndex()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            var dbContext = new LoComproContext(options);
            var model = new AnomaliesModel(dbContext);
            int recordCount = 8; // Un n�mero par de registros para el ejemplo

            int result = model.CalculateQ2Index(recordCount);

            Assert.That(result, Is.EqualTo(4));
        }

        // Test by Sebasti�n Rodr�guez Tencio - C06756. Sprint 3
        [Test]
        public void CalculateQ2Index_OddCount_ReturnsMiddleIndex()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            var dbContext = new LoComproContext(options);
            var model = new AnomaliesModel(dbContext);
            int recordCount = 7; // Un n�mero impar de registros para el ejemplo

            int result = model.CalculateQ2Index(recordCount);

            Assert.That(result, Is.EqualTo(4));
        }

        // Test by Sebasti�n Rodr�guez Tencio - C06756. Sprint 3
        [Test]
        public void CalculateQ1Index_EvenQ2_ReturnsLowerQuartileIndex()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            var dbContext = new LoComproContext(options);
            var model = new AnomaliesModel(dbContext);
            int q2 = 8; // Un �ndice par de Q2 para el ejemplo

            int result = model.CalculateQ1Index(q2);

            Assert.That(result, Is.EqualTo(3));
        }

        // Test by Sebasti�n Rodr�guez Tencio - C06756. Sprint 3
        [Test]
        public void CalculateQ1Index_OddQ2_ReturnsLowerQuartileIndex()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            var dbContext = new LoComproContext(options);
            var model = new AnomaliesModel(dbContext);
            int q2 = 11; // Un �ndice impar de Q2 para el ejemplo

            int result = model.CalculateQ1Index(q2);

            Assert.That(result, Is.EqualTo(4));
        }

        // Test by Sebasti�n Rodr�guez Tencio - C06756. Sprint 3
        [Test]
        public void CalculateQ3Index_EvenQ2EvenCount_ReturnsUpperQuartileIndex()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            var dbContext = new LoComproContext(options);
            var model = new AnomaliesModel(dbContext);
            int q2 = 4; // Un �ndice par de Q2 para el ejemplo
            int recordCount = 8; // Un n�mero par de registros para el ejemplo

            int result = model.CalculateQ3Index(q2, recordCount);

            Assert.That(result, Is.EqualTo(6)); // Ajusta el resultado esperado seg�n tu l�gica
        }

        // Test by Sebasti�n Rodr�guez Tencio - C06756. Sprint 3
        [Test]
        public void CalculateQ3Index_OddQ2OddCount_ReturnsUpperQuartileIndex()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            var dbContext = new LoComproContext(options);
            var model = new AnomaliesModel(dbContext);
            int q2 = 5; // Un �ndice impar de Q2 para el ejemplo
            int recordCount = 9; // Un n�mero impar de registros para el ejemplo

            int result = model.CalculateQ3Index(q2, recordCount);

            Assert.That(result, Is.EqualTo(8));
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
                NameProvince = "San Jos�"
            };

            var canton = new Canton
            {
                NameCanton = "Tib�s",
                Province = province,
                NameProvince = "San Jos�"
            };

            var store = new Store
            {
                NameStore = "Ishop",
                Canton = canton,
                NameProvince = "San Jos�",
                NameCanton = "Tib�s",
                Latitude = 9.9516,
                Longitude = -84.0990
            };

            var record1 = new Record
            {
                NameGenerator = "anne",
                GeneratorUser = generatorUser,
                RecordDate = DateTime.Parse("2022-01-18"),
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

            if (recordsCount > 6)
            {
                // Crear algunos datos simulados para usar en la lista agrupada
                var groupKey1 = new GroupingKey { NameProduct = "Apple Iphone 11", NameStore = "Ishop", NameCanton = "Tib�s", NameProvince = "San Jos�" };
                var recordToGroup1 = new RecordStoreModel { Record = record1, Store = store };
                var recordToGroup2 = new RecordStoreModel { Record = record2, Store = store };
                var recordToGroup3 = new RecordStoreModel { Record = record2, Store = store };
                var recordToGroup4 = new RecordStoreModel { Record = record2, Store = store };
                var recordToGroup5 = new RecordStoreModel { Record = record2, Store = store };
                var recordToGroup6 = new RecordStoreModel { Record = record2, Store = store };
                var recordToGroup7 = new RecordStoreModel { Record = record2, Store = store };
                var recordToGroup8 = new RecordStoreModel { Record = record2, Store = store };

                var group1 = new List<RecordStoreModel> { recordToGroup1, recordToGroup2, recordToGroup3, recordToGroup4, recordToGroup5, recordToGroup6, recordToGroup7, recordToGroup8 };
                var grouping1 = group1.GroupBy(r => groupKey1);

                groupedRecords.AddRange(grouping1);
            }
            else
            {
                var groupKey1 = new GroupingKey { NameProduct = "Apple Iphone 11", NameStore = "Ishop", NameCanton = "Tib�s", NameProvince = "San Jos�" };
                var recordToGroup1 = new RecordStoreModel { Record = record1, Store = store };
                var recordToGroup2 = new RecordStoreModel { Record = record2, Store = store };
                var recordToGroup3 = new RecordStoreModel { Record = record2, Store = store };
                var recordToGroup4 = new RecordStoreModel { Record = record2, Store = store };
                var recordToGroup5 = new RecordStoreModel { Record = record2, Store = store };

                var group1 = new List<RecordStoreModel> { recordToGroup1, recordToGroup2, recordToGroup3, recordToGroup4, recordToGroup5 };
                var grouping1 = group1.GroupBy(r => groupKey1);

                groupedRecords.AddRange(grouping1);
            }

            return groupedRecords;
        }
    }
}
