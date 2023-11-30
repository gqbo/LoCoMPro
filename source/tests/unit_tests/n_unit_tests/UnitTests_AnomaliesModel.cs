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
        // Test by Sebastián Rodríguez Tencio - C06756. Sprint 3
        [Test]
        public void CalculateQ2Index_EvenCount_ReturnsMiddleIndex()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            var dbContext = new LoComproContext(options);
            var model = new AnomaliesModel(dbContext);
            int recordCount = 8; // Un número par de registros para el ejemplo

            int result = model.CalculateQ2Index(recordCount);

            Assert.That(result, Is.EqualTo(4));
        }

        // Test by Sebastián Rodríguez Tencio - C06756. Sprint 3
        [Test]
        public void CalculateQ2Index_OddCount_ReturnsMiddleIndex()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            var dbContext = new LoComproContext(options);
            var model = new AnomaliesModel(dbContext);
            int recordCount = 7; // Un número impar de registros para el ejemplo

            int result = model.CalculateQ2Index(recordCount);

            Assert.That(result, Is.EqualTo(4));
        }

        // Test by Sebastián Rodríguez Tencio - C06756. Sprint 3
        [Test]
        public void CalculateQ1Index_EvenQ2_ReturnsLowerQuartileIndex()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            var dbContext = new LoComproContext(options);
            var model = new AnomaliesModel(dbContext);
            int q2 = 8; // Un índice par de Q2 para el ejemplo

            int result = model.CalculateQ1Index(q2);

            Assert.That(result, Is.EqualTo(3));
        }

        // Test by Sebastián Rodríguez Tencio - C06756. Sprint 3
        [Test]
        public void CalculateQ1Index_OddQ2_ReturnsLowerQuartileIndex()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            var dbContext = new LoComproContext(options);
            var model = new AnomaliesModel(dbContext);
            int q2 = 11; // Un índice impar de Q2 para el ejemplo

            int result = model.CalculateQ1Index(q2);

            Assert.That(result, Is.EqualTo(4));
        }

        // Test by Sebastián Rodríguez Tencio - C06756. Sprint 3
        [Test]
        public void CalculateQ3Index_EvenQ2EvenCount_ReturnsUpperQuartileIndex()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            var dbContext = new LoComproContext(options);
            var model = new AnomaliesModel(dbContext);
            int q2 = 4; // Un índice par de Q2 para el ejemplo
            int recordCount = 8; // Un número par de registros para el ejemplo

            int result = model.CalculateQ3Index(q2, recordCount);

            Assert.That(result, Is.EqualTo(6)); // Ajusta el resultado esperado según tu lógica
        }

        // Test by Sebastián Rodríguez Tencio - C06756. Sprint 3
        [Test]
        public void CalculateQ3Index_OddQ2OddCount_ReturnsUpperQuartileIndex()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            var dbContext = new LoComproContext(options);
            var model = new AnomaliesModel(dbContext);
            int q2 = 5; // Un índice impar de Q2 para el ejemplo
            int recordCount = 9; // Un número impar de registros para el ejemplo

            int result = model.CalculateQ3Index(q2, recordCount);

            Assert.That(result, Is.EqualTo(8));
        }
    }
}
