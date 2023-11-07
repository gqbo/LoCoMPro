using LoCoMPro_LV.Areas.Identity.Pages.Account;
using LoCoMPro_LV.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace nunit_tests
{
    [TestFixture]
    public class UnitTests_RegisterModel: TestUtils
    {
        // Test by Gabriel Gonz�lez Flores - C03376
        [Test]
        public void OnPostAsync_InvalidLocation_ReturnsViewResult()
        {
            // Arrange
            var model = ConfigurePageModel("register_page") as RegisterModel;
            model.NameCanton = "N/A";
            model.NameProvince = "N/A";

            // Act
            var result = model.OnPostAsync().Result;

            // Assert
            Assert.IsInstanceOf<PageResult>(result);
        }

        // Test by Gabriel Gonz�lez Flores - C03376
        [Test]
        public void HandlePasswordErrors_PasswordRequiresDigit()
        {
            // Arrange
            var model = ConfigurePageModel("register_page") as RegisterModel;

            var modelState = new ModelStateDictionary();
            var identityResult = IdentityResult.Failed(
                new IdentityError { Code = "PasswordRequiresDigit" }
            );

            // Act
            model.HandlePasswordErrors(identityResult);

            // Assert
            Assert.IsTrue(model.ModelState[""].Errors.Any(error => error.ErrorMessage == "La contrase�a debe contener al menos un n�mero."));
        }

        // Test by Gabriel Gonz�lez Flores - C03376
        [Test]
        public void HandlePasswordErrors_PasswordTooShort()
        {
            // Arrange
            var model = ConfigurePageModel("register_page") as RegisterModel;
            var modelState = new ModelStateDictionary();
            var identityResult = IdentityResult.Failed(
                new IdentityError { Code = "PasswordTooShort" }
            );

            // Act
            model.HandlePasswordErrors(identityResult);

            // Assert
            Assert.IsTrue(model.ModelState[""].Errors.Any(error => error.ErrorMessage == "La contrase�a debe tener una longitud m�nima de 6 caracteres."));
        }

        // Test by Gabriel Gonz�lez Flores - C03376
        [Test]
        public void HandlePasswordErrors_PasswordRequiresUniqueChars()
        {
            // Arrange
            var model = ConfigurePageModel("register_page") as RegisterModel;
            var modelState = new ModelStateDictionary();
            var identityResult = IdentityResult.Failed(
                new IdentityError { Code = "PasswordRequiresUniqueChars" }
            );

            // Act
            model.HandlePasswordErrors(identityResult);

            // Assert
            Assert.IsTrue(model.ModelState[""].Errors.Any(error => error.ErrorMessage == "La contrase�a debe contener al menos un car�cter �nico."));
        }

        // Test by Gabriel Gonz�lez Flores - C03376
        [Test]
        public void HandlePasswordErrors_PasswordRequiresUpper()
        {
            // Arrange
            var model = ConfigurePageModel("register_page") as RegisterModel;
            var modelState = new ModelStateDictionary();
            var identityResult = IdentityResult.Failed(
                new IdentityError { Code = "PasswordRequiresUpper" }
            );

            // Act
            model.HandlePasswordErrors(identityResult);

            // Assert
            Assert.IsTrue(model.ModelState[""].Errors.Any(error => error.ErrorMessage == "La contrase�a debe contener al menos un car�cter en may�scula."));
        }

        // Test by Gabriel Gonz�lez Flores - C03376
        [Test]
        public void CreateUser_IsNotNull()
        {
            // Arrange
            var model = ConfigurePageModel("register_page") as RegisterModel;

            // Act
            var user = model.CreateUser();

            // Assert
            Assert.IsNotNull(user);
        }

        // Test by Gabriel Gonz�lez Flores - C03376
        [Test]
        public void CreateUser_IsInstanceOf()
        {
            // Arrange
            var model = ConfigurePageModel("register_page") as RegisterModel;

            // Act
            var user = model.CreateUser();

            // Assert
            Assert.IsInstanceOf<ApplicationUser>(user);
        }
    }
}