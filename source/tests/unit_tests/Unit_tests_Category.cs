using LoCoMPro_LV.Areas.Identity.Pages.Account;
using LoCoMPro_LV.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace UnitTests_Category
{

    [TestClass]
    public class UnitTests_Category
    {
        // Test by Cristopher Hernandez Calderon - C13632
        [TestMethod]
        public void Category_NameCategory_ValidCategoryName()
        {
            var productModel = new Category { NameCategory = "AK-47" };
            var context = new ValidationContext(productModel);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(productModel, context, results, true);

            Assert.IsTrue(isValid);
            Assert.IsFalse(results.Any(r => r.MemberNames.Contains("NameCategory")), "El nombre de la categoria esta correcto ya que cumple todos los parametros necesarios.");
        }

        // Test by Cristopher Hernandez Calderon - C13632
        [TestMethod]
        public void Category_NameCategory_NoCategoryName()
        {
            var productModel = new Category { };
            var context = new ValidationContext(productModel);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(productModel, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("NameCategory")), "El nombre de la categoria es necesario");
        }

        // Test by Cristopher Hernandez Calderon - C13632
        [TestMethod]
        public void Category_NameCategory_InvalidDontHaveLetters()
        {
            var productModel = new Category { NameCategory = "341646841#" };
            var context = new ValidationContext(productModel);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(productModel, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("NameCategory")), "El nombre de la categoria debe ser inválido debido a que no incluye ninguna letra ya sea mayuscula o minuscula.");
        }

        // Test by Cristopher Hernandez Calderon - C13632
        [TestMethod]
        public void Category_NameTopCategory_InvalidDontHaveLetters()
        {
            var productModel = new Category { NameCategory = "AK-47", NameTopCategory = "641416.#$%" };
            var context = new ValidationContext(productModel);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(productModel, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("NameTopCategory")), "El nombre de la categoria superior debe ser inválido debido a que no incluye ninguna letra ya sea mayuscula o minuscula.");
        }

        // Test by Cristopher Hernandez Calderon - C13632
        [TestMethod]
        public void Category_NameTopCategory_ValidTopCategoryName()
        {
            var productModel = new Category { NameCategory = "AK-47", NameTopCategory = "Armas" };
            var context = new ValidationContext(productModel);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(productModel, context, results, true);

            Assert.IsTrue(isValid);
            Assert.IsFalse(results.Any(r => r.MemberNames.Contains("NameTopCategory")), "El nombre de la categoria superior esta correcto ya que cumple todos los parametros necesarios.");
        }
    }
}
