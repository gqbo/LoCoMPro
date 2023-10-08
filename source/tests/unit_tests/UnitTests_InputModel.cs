using LoCoMPro_LV.Areas.Identity.Pages.Account;
using System.ComponentModel.DataAnnotations;

namespace UnitTests_InputModel
{
    [TestClass]
    public class UnitTests_InputModel
    {
        [TestMethod]
        public void RegisterModel_Email_Invalid()
        {
            var InputModel = new RegisterModel.InputModel { Email = "prueba.gmail.com" };

            var context = new ValidationContext(InputModel);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(InputModel, context, results, true);

            Assert.IsFalse(isValid, "Email debe ser inválido debido a que no es una correo electrónico válido.");
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("Email")), "Debe haber un error de validación para Email.");
        }
    }
}