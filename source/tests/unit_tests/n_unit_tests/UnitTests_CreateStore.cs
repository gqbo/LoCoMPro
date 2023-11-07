using Microsoft.AspNetCore.Mvc;
using LoCoMPro_LV.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LoCoMPro_LV.Pages.Stores;
using LoCoMPro_LV.Utils;

namespace n_unit_tests
{
    [TestFixture]
    public class UnitTests_CreateStore : TestUtils
    {
        // Test by Yordi Robles Siles - C06557
        [Test]
        public void OnPostAsyncInvalidLocationNameCanton()
        {
            var model = ConfigurePageModel("createStore_page") as CreateStoreModel;
            model.Store = new Store
            {
                NameStore = "Walmart",
                NameCanton = "N/A",
                NameProvince = "Puntarenas",
                Latitude = 8.9502,
                Longitude = -84.0399
            };

            var result = model.OnPostAsync();

            Assert.That(result, Is.InstanceOf<PageResult>());
        }

        // Test by Yordi Robles Siles - C06557
        [Test]
        public void OnPostAsyncInvalidLocationNameProvinceAndCanton()
        {
            var model = ConfigurePageModel("createStore_page") as CreateStoreModel;
            model.Store = new Store
            {
                NameStore = "Walmart",
                NameCanton = "N/A",
                NameProvince = "N/A",
                Latitude = 9.5032,
                Longitude = -82.4084
            };

            var result = model.OnPostAsync();

            Assert.That(result, Is.InstanceOf<PageResult>());
        }

        // Test by Yordi Robles Siles - C06557
        [Test]
        public void OnPostAsyncValidLocation()
        {
            var model = ConfigurePageModel("createStore_page") as CreateStoreModel;
            model.Store = new Store
            {
                NameStore = "Walmart",
                NameCanton = "San Ramón",
                NameProvince = "Alajuela",
                Latitude = 10.2825,
                Longitude = -84.7321
            };

            var result = model.OnPostAsync();

            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
        }

        // Test by Yordi Robles Siles - C06557
        [Test]
        public void  CalculateDistanceLessThan()
        {
            double lat1 = 9.9281;
            double lon1 = -84.0907;
            double lat2 = 9.9293;
            double lon2 = -84.0905;
            var distance = Geolocation.CalculateDistance(lat1, lon1, lat2, lon2);
            Assert.That(distance <= 2000, Is.True);
        }

        // Test by Yordi Robles Siles - C06557
        [Test]
        public void CalculateDistanceMoreThan()
        {
            double lat1 = 9.9614;
            double lon1 = -84.0818;
            double lat2 = 9.9617;
            double lon2 = -84.0622;
            var distance = Geolocation.CalculateDistance(lat1, lon1, lat2, lon2);
            Assert.That(distance >= 2000, Is.True);
        }

        // Test by Yordi Robles Siles - C06557
        [Test]
        public void DegreeToRadiansIncorrectValue()
        {
            var radians = Geolocation.DegreesToRadians(9.9614);
            Assert.That(radians == 0.17385922811, Is.False);

        }
    }
}
