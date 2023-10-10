using LoCoMPro_LV.Areas.Identity.Pages.Account;
using LoCoMPro_LV.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace UnitTests_Store
{
    [TestClass]
    public class UnitTests_Store
    {
        // Test by Sebastián Rodríguez Tencio - C06756
        [TestMethod]
        public void Store_NameStore_ValidLength()
        {
            var storeModel = new Store
            {
                NameStore = "Mi tienda",
                NameProvince = "San José",
                NameCanton = "Tibás"
            };
            var context = new ValidationContext(storeModel);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(storeModel, context, results, true);

            Assert.IsTrue(isValid);
        }

        // Test by Sebastián Rodríguez Tencio - C06756
        [TestMethod]
        public void Store_NameStore_InvalidMinLength()
        {
            var storeModel = new Store
            {
                NameStore = "a",
                NameProvince = "San José",
                NameCanton = "Tibás"
            };
            var context = new ValidationContext(storeModel);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(storeModel, context, results, true);

            Assert.IsFalse(isValid);
        }

        // Test by Sebastián Rodríguez Tencio - C06756
        [TestMethod]
        public void Store_NameStore_InvalidMaxLength()
        {
            var storeModel = new Store
            {
                NameStore = new string('A', 101),
                NameProvince = "San José",
                NameCanton = "Tibás"
            };
            var context = new ValidationContext(storeModel);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(storeModel, context, results, true);

            Assert.IsFalse(isValid);
        }
        
        // Test by Sebastián Rodríguez Tencio - C06756
        [TestMethod]
        public void Store_NameStore_ValidCharacters()
        {
            var storeModel = new Store
            {
                NameStore = "ABC123 ,.-()%:#áéíóúÁÉÍÓÚ",
                NameProvince = "San José",
                NameCanton = "Tibás"
            };
            var context = new ValidationContext(storeModel);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(storeModel, context, results, true);

            Assert.IsTrue(isValid);
        }

        // Test by Sebastián Rodríguez Tencio - C06756
        [TestMethod]
        public void Store_NameStore_InvalidCharacters()
        {
            var storeModel = new Store
            {
                NameStore = "!@#$%^&",
                NameProvince = "San José",
                NameCanton = "Tibás"
            };
            var context = new ValidationContext(storeModel);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(storeModel, context, results, true);

            Assert.IsFalse(isValid);
        }

        // Test by Sebastián Rodríguez Tencio - C06756
        [TestMethod]
        public void Store_NameStore_EmptyValue()
        {
            var storeModel = new Store
            {
                NameStore = "",
                NameProvince = "San José",
                NameCanton = "Tibás"
            };
            var context = new ValidationContext(storeModel);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(storeModel, context, results, true);

            Assert.IsFalse(isValid);
        }

        // Test by Sebastián Rodríguez Tencio - C06756
        [TestMethod]
        public void Store_NameStore_ValidWithAccents()
        {
            var storeModel = new Store
            {
                NameStore = "Café y Té",
                NameProvince = "San José",
                NameCanton = "Tibás"
            };
            var context = new ValidationContext(storeModel);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(storeModel, context, results, true);

            Assert.IsTrue(isValid);
        }

        // Test by Sebastián Rodríguez Tencio - C06756
        [TestMethod]
        public void Store_NameStore_InvalidNoLetters()
        {
            var storeModel = new Store
            {
                NameStore = "12345",
                NameProvince = "San José",
                NameCanton = "Tibás"
            };
            var context = new ValidationContext(storeModel);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(storeModel, context, results, true);

            Assert.IsFalse(isValid);
        }
    }
}