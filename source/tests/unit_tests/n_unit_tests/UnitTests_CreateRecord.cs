using LoCoMPro_LV.Models;
using LoCoMPro_LV.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using LoCoMPro_LV.Utils;
using LoCoMPro_LV.Pages.Records ;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace n_unit_tests
{
    [TestFixture]
    public class UnitTests_CreateRecord
    {
        // Test by Gabriel González Flores - C03376. Sprint 3
        [Test]
        public async Task LoadStoresAsync_Valid()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var dbContext = new LoComproContext(options);

            var store = new Store
            {
                NameStore = "Store1",
                NameProvince = "San José",
                NameCanton = "Tibás",
                Latitude = 9.9513,
                Longitude = -84.0991
            };

            var store2 = new Store
            {
                NameStore = "Store2",
                NameProvince = "Heredia",
                NameCanton = "Heredia",
                Latitude = 9.9513,
                Longitude = -84.0991
            };

            dbContext.Add(store);
            dbContext.Add(store2);
            dbContext.SaveChanges();

            var model = new CreateModel(dbContext, signInManager: null);

            await model.LoadStoresAsync();

            Assert.IsNotNull(model.Stores);
            Assert.That(model.Stores.Count, Is.EqualTo(2));
        }

        // Test by Gabriel González Flores - C03376. Sprint 3
        [Test]
        public async Task LoadStoresAsync_Invalid()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var dbContext = new LoComproContext(options);

            var model = new CreateModel(dbContext, signInManager: null);

            await model.LoadStoresAsync();

            Assert.IsNotNull(model.Stores);
            Assert.That(model.Stores.Count, Is.EqualTo(0));
        }

        // Test by Gabriel González Flores - C03376. Sprint 3
        [Test]
        public async Task LoadProductsAsync_Valid()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var dbContext = new LoComproContext(options);

            var product = new Product
            {
                NameProduct = "Apple Iphone 11 64gb"
            };

            dbContext.Add(product);
            dbContext.SaveChanges();

            var model = new CreateModel(dbContext, signInManager: null);

            await model.LoadProductsAsync();

            Assert.IsNotNull(model.Product);
            Assert.That(model.Product.Count, Is.EqualTo(1));
            Assert.Contains("Apple Iphone 11 64gb", model.Product);
        }

        // Test by Gabriel González Flores - C03376. Sprint 3
        [Test]
        public async Task LoadProductsAsync_Invalid()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var dbContext = new LoComproContext(options);

            var model = new CreateModel(dbContext, signInManager: null);

            await model.LoadProductsAsync();

            Assert.IsNotNull(model.Product);
            Assert.That(model.Product.Count, Is.EqualTo(0));
        }

        // Test by Yordi Robles Siles - C06557. Sprint 3
        [Test]
        public async Task LoadCategoriesAsync_Invalid()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var dbContext = new LoComproContext(options);

            var model = new CreateModel(dbContext, signInManager: null);

            await model.LoadCategoriesAsync();

            Assert.IsNotNull(model.Categories);
            Assert.That(model.Categories.Count, Is.EqualTo(0));
        }

        // Test by Yordi Robles Siles - C06557. Sprint 3
        [Test]
        public async Task LoadCategoriesAsync_Valid()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<LoComproContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var dbContext = new LoComproContext(options);
            Category category1 = new Category { NameCategory = "Carros", NameTopCategory = "Juguetes" };
            Category category2 = new Category { NameCategory = "Motos", NameTopCategory = "Juguetes" };
            dbContext.Categories.Add(category1);
            dbContext.Categories.Add(category2);
            dbContext.SaveChanges();
            var model = new CreateModel(dbContext, signInManager: null);

            await model.LoadCategoriesAsync();

            Assert.IsNotNull(model.Categories);
            Assert.That(model.Categories.Count, Is.EqualTo(2));
        }
    }

}
