using System.ComponentModel.DataAnnotations;
using LoCoMPro_LV.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LoCoMPro_LV.Areas.Identity.Pages.Account.Manage
{
    /// <summary>
    /// Clase que maneja la lógica relacionada el gestionamiento de una cuenta
    /// con respecto al email
    /// </summary>
    public class EmailModel : PageModel
    {
        /// <summary>
        /// Administra a los usuarios de tipo ApplicationUser.
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;
        /// <summary>
        /// Gestiona el refresh para el inicio de sesión de los usuarios.
        /// </summary>
        private readonly SignInManager<ApplicationUser> _signInManager;

        public EmailModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///  Almacena el email del usuario que desea gestionar su cuenta.
        /// </summary>
        public string Email { get; set; }

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
            [EmailAddress]
            [Display(Name = "Nuevo correo electrónico")]
            public string NewEmail { get; set; }
        }

        /// <summary>
        /// Método utilizado para cargar en el InputModel los datos actuales de un usuario.
        /// </summary>
        /// <param name="user">Usuario actual que desea gestionar su cuenta</param>
        private async Task LoadAsync(ApplicationUser user)
        {
            var email = await _userManager.GetEmailAsync(user);
            Email = email;

            Input = new InputModel
            {
                NewEmail = email,
            };
        }

        /// <summary>
        /// Método invocado cuando se realiza una solicitud GET que prepara la página de gestionar una cuenta relacionado con el email.
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
        public async Task<IActionResult> OnPostChangeEmailAsync()
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

            var email = await _userManager.GetEmailAsync(user);
            if (Input.NewEmail != email)
            {
                var result = await _userManager.SetEmailAsync(user, Input.NewEmail);

                if (result.Succeeded)
                {
                    await _userManager.UpdateAsync(user);
                    await _signInManager.RefreshSignInAsync(user);
                    StatusMessage = "Se ha actualizado el correo electrónico";
                }
            }
            return RedirectToPage();
        }
    }
}
