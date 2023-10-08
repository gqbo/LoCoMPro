using LoCoMPro_LV.Areas.Identity.Pages.Account;
using System.ComponentModel.DataAnnotations;

namespace UnitTests_InputModel
{
    [TestClass]
    public class UnitTests_InputModel
    {
        // Test by Gabriel Gonz�lez Flores - C03376
        [TestMethod]
        public void RegisterModel_Email_Invalid()
        {
            var InputModel = new RegisterModel.InputModel 
            { 
                Email = "gabriel.gmail.com" 
            };
            var context = new ValidationContext(InputModel);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(InputModel, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("Email")), "Email debe ser inv�lido debido a que no es un correo electr�nico v�lido.");
        }

        // Test by Gabriel Gonz�lez Flores - C03376
        [TestMethod]
        public void RegisterModel_Email_Required()
        {
            var InputModel = new RegisterModel.InputModel 
            { 
                UserName = "gabriel", 
                FirstName = "Gabriel", 
                LastName = "Gonz�lez", 
                Password = "Gabriel1.", 
                ConfirmPassword = "Gabriel1."
            };
            var context = new ValidationContext(InputModel);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(InputModel, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("Email")), "Email debe ser inv�lido debido a que es obligatorio.");
          
        }

        // Test by Gabriel Gonz�lez Flores - C03376
        [TestMethod]
        public void RegisterModel_UserName_InvalidMinimumLength()
        {
            var InputModel = new RegisterModel.InputModel 
            { 
                UserName = "g"
            };
            var context = new ValidationContext(InputModel);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(InputModel, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("UserName")), "El nombre de usuario debe ser inv�lido debido a que no contiene m�s de 2 caracteres.");
        }

        // Test by Gabriel Gonz�lez Flores - C03376
        [TestMethod]
        public void RegisterModel_UserName_InvalidMaximumLength()
        {
            var InputModel = new RegisterModel.InputModel 
            {
                UserName = "gabrielgabrielgabrielgabrielgabrielgabrielgabrielgabriel"
            };
            var context = new ValidationContext(InputModel);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(InputModel, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("UserName")), "El nombre de usuario debe ser inv�lido debido a que contiene m�s de 50 caracteres.");
        }

        // Test by Gabriel Gonz�lez Flores - C03376
        [TestMethod]
        public void RegisterModel_UserName_InvalidSpecialChar()
        {
            var InputModel = new RegisterModel.InputModel
            {
                UserName = "gabriel$"
            };
            var context = new ValidationContext(InputModel);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(InputModel, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("UserName")), "El nombre de usuario debe ser inv�lido debido a que contiene un caracter especial.");
        }

        // Test by Gabriel Gonz�lez Flores - C03376
        [TestMethod]
        public void RegisterModel_UserName_Required()
        {
            var InputModel = new RegisterModel.InputModel
            {
                Email = "gabriel@gmail.com",
                FirstName = "Gabriel",
                LastName = "Gonz�lez",
                Password = "Gabriel1.",
                ConfirmPassword = "Gabriel1."
            };
            var context = new ValidationContext(InputModel);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(InputModel, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("UserName")), "El nombre de usuario debe ser inv�lido debido a que es obligatorio.");
        }

        // Test by Gabriel Gonz�lez Flores - C03376
        [TestMethod]
        public void RegisterModel_Password_InvalidMinimumLength()
        {
            var InputModel = new RegisterModel.InputModel
            {
                Password = "G"
            };
            var context = new ValidationContext(InputModel);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(InputModel, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("Password")), "La contrase�a debe ser inv�lida debido a que no contiene m�s de 2 caracteres.");
        }

        // Test by Gabriel Gonz�lez Flores - C03376
        [TestMethod]
        public void RegisterModel_Password_InvalidMaximumLength()
        {
            var InputModel = new RegisterModel.InputModel
            {
                Password = "Gabriel1Gabriel1Gabriel1Gabriel1Gabriel1Gabriel1Gabriel1Gabriel1Gabriel1Gabriel1Gabriel1"
            };
            var context = new ValidationContext(InputModel);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(InputModel, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("Password")), "La contrase�a debe ser inv�lida debido a que contiene m�s de 50 caracteres.");
        }

        // Test by Gabriel Gonz�lez Flores - C03376
        [TestMethod]
        public void RegisterModel_Password_InvalidNoNumbers()
        {
            var InputModel = new RegisterModel.InputModel
            {
                Password = "Password"
            };
            var context = new ValidationContext(InputModel);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(InputModel, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("Password")), "La contrase�a debe ser inv�lida debido a no contiene n�meros.");
        }

        // Test by Gabriel Gonz�lez Flores - C03376
        [TestMethod]
        public void RegisterModel_Password_InvalidNoUpperCase()
        {
            var InputModel = new RegisterModel.InputModel
            {
                Password = "password"
            };
            var context = new ValidationContext(InputModel);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(InputModel, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("Password")), "La contrase�a debe ser inv�lida debido a no contiene may�sculas.");
        }

        // Test by Gabriel Gonz�lez Flores - C03376
        [TestMethod]
        public void RegisterModel_Password_Required()
        {
            var InputModel = new RegisterModel.InputModel
            {
                Email = "gabriel@gmail.com",
                UserName = "gabriel",
                FirstName = "Gabriel",
                LastName = "Gonz�lez"
            };
            var context = new ValidationContext(InputModel);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(InputModel, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("Password")), "La contrase�a debe ser inv�lida debido a que es obligatorio.");
        }
    }
}