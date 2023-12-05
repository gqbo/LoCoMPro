using System.ComponentModel.DataAnnotations;
using LoCoMPro_LV.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LoCoMPro_LV.Areas.Identity.Pages.Account.Manage
{
    /// <summary>
    /// Clase que maneja la lógica relacionada el gestionamiento de una cuenta
    /// con respecto al nombre y el apellido
    /// </summary>
    public class IndexModel : PageModel
    {
        /// <summary>
        /// Administra a los usuarios de tipo ApplicationUser.
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;
        /// <summary>
        /// Gestiona el refresh para el inicio de sesión de los usuarios.
        /// </summary>
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///  Almacena el nombre de usuario del usuario que desea gestionar su cuenta.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///  Se utiliza para informar al usuario del status de sus cambios.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        /// Modelo que se utiliza para recopilar los datos que el usuario ingresa a la hora de gestionar su cuenta.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        /// Clase que se utiliza para definir los datos que se recopilan a la hora de gestionar una cuenta.
        /// </summary>
        public class InputModel
        {
            [PersonalData]
            [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres.")]
            [RegularExpression(@"^[a-zA-ZáéíóúñÑÁÉÍÓÚ]+$", ErrorMessage = "El nombre debe contener solo letras (mayúsculas o minúsculas).")]
            [Display(Name = "Nombre")]
            public string FirstName { get; set; }

            [PersonalData]
            [StringLength(50, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 50 caracteres.")]
            [RegularExpression(@"^[a-zA-ZáéíóúñÑÁÉÍÓÚ]+$", ErrorMessage = "El apellido debe contener solo letras (mayúsculas o minúsculas).")]
            [Display(Name = "Apellido")]
            public string LastName { get; set; }
        }

        /// <summary>
        /// Método utilizado para cargar en el InputModel los datos actuales de un usuario.
        /// </summary>
        /// <param name="user">Usuario actual que desea gestionar su cuenta</param>
        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var firstName = user.FirstName;
            var lastName = user.LastName;

            Username = userName;

            Input = new InputModel
            {
                FirstName = firstName,
                LastName = lastName
            };
        }

        /// <summary>
        /// Método invocado cuando se realiza una solicitud GET que prepara la página de gestionar una cuenta relacionado con la información personal
        /// del perfil.
        /// </summary>
        public async Task<IActionResult> OnGetAsync()
        {
            var AuthenticatedUserName = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(AuthenticatedUserName);
            if (user == null)
            {
                return NotFound($"Unable to load user with UserName: '{AuthenticatedUserName}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        /// <summary>
        /// Método invocado cuando se realiza una solicitud POST que modifica los valores actuales de un usuario con 
        /// los valores ingresados en el formulario.
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            var AuthenticatedUserName = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(AuthenticatedUserName);
            if (user == null)
            {
                return NotFound($"Unable to load user with UserName: '{AuthenticatedUserName}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var firstNameBefore = user.FirstName;
            var lastNameBefore = user.LastName;

            if (Input.FirstName != firstNameBefore)
            {
                user.FirstName = Input.FirstName;
            }

            if (Input.LastName != lastNameBefore)
            {
                user.LastName = Input.LastName;
            }

            if (Input.FirstName != firstNameBefore || Input.LastName != lastNameBefore)
            {
                await _userManager.UpdateAsync(user);
                await _signInManager.RefreshSignInAsync(user);
                StatusMessage = "Se ha actualizado el perfil";
            }

            return RedirectToPage();
        }
    }
}
