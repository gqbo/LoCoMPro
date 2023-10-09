using LoCoMPro_LV.Areas.Identity.Pages.Account;
using LoCoMPro_LV.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace UnitTests_Product
{
    [TestClass]
    public class UnitTests_Product
    {
        // Test by Yordi Robles Siles - C06557
        [TestMethod]
        public void Product_NameProduct_InvalidMinimunLength()
        {
            var productModel = new Product { NameProduct = "a" };
            var context = new ValidationContext(productModel);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(productModel, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("NameProduct")), "El nombre del producto debe ser inválido debido a que incumple la cantidad mínima de caracteres.");
        }

        // Test by Yordi Robles Siles - C06557
        [TestMethod]
        public void Product_NameProduct_InvalidMaximumLength()
        {
            var productModel = new Product { NameProduct = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" };
            var context = new ValidationContext(productModel);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(productModel, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("NameProduct")), "El nombre del producto debe ser inválido debido a que incumple la cantidad máxima de caracteres.");
        }

        // Test by Yordi Robles Siles - C06557
        [TestMethod]
        public void Product_NameProduct_Required()
        {
            var productModel = new Product { };
            var context = new ValidationContext(productModel);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(productModel, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("NameProduct")), "El nombre del producto debe ser invalido debido a que es obligatorio");
        }

        // Test by Yordi Robles Siles - C06557
        [TestMethod]
        public void Product_NameProduct_InvalidRegex()
        {
            var productModel = new Product { NameProduct = "Arroz12" };
            var context = new ValidationContext(productModel);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(productModel, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("NameProduct")), "El nombre del producto debe ser invalido debido a que contiene números");
        }

        // Test by Yordi Robles Siles - C06557
        [TestMethod]
        public void Product_NameProduct_ValidRegex()
        {
            var productModel = new Product { NameProduct = "Pc Gamer" };
            var context = new ValidationContext(productModel);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(productModel, context, results, true);

            Assert.IsTrue(isValid);
           
        }
    }
}