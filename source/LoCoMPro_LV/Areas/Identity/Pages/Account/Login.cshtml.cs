// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using LoCoMPro_LV.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LoCoMPro_LV.Areas.Identity.Pages.Account
{
    /// <summary>
    /// Clase que maneja la lógica relacionada con el inicio de sesión de usuarios en la aplicación web.
    /// Recopila la información ingresada y valida con respecto a la información almacenada en la base de datos sobre los usuarios registrados en la aplicación web.
    /// </summary>
    public class LoginModel : PageModel
    {
        /// <summary>
        /// Gestiona el inicio de sesión de los usuarios.
        /// </summary>
        private readonly SignInManager<ApplicationUser> _signInManager;
        /// <summary>
        ///  Registra información, advertencias y errores.
        /// </summary>
        private readonly ILogger<LoginModel> _logger;

        /// <summary>
        /// Constructor de la clase LoginModel.
        /// Se encarga de inicializar los atributos de la clase para el manejo de usuarios.
        /// </summary>
        /// <param name="signInManager">Gestiona el inicio de sesión de los usuarios.</param>
        /// <param name="logger">Registra información, advertencias y errores.</param>
        public LoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        /// <summary>
        /// Modelo que se utiliza para recopilar los datos que el usuario ingresa en el formulario de registro de usuarios.
        /// </summary>
        public InputModel Input { get; set; }

        /// <summary>
        /// Propiedad que almacena una lista de esquemas de autenticación externa disponibles.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        /// Atributo que almacena el URL a donde se va a redirigir después de registrar un usuario.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///  Atributo que se utiliza para mostrar un mensaje de error en caso de que haya ocurrido un intento de inicio de sesión fallido
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Clase que se utiliza para definir los datos que se recopilan en el formulario de inicio de sesión de usuarios y sus restricciones.
        /// </summary>
        public class InputModel
        {
            [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
            [DataType(DataType.Text)]
            [Display(Name = "Usuario")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "La contraseña es obligatoria.")]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña")]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        /// <summary>
        /// Método invocado cuando se realiza una solicitud GET que se encarga de configurar y preparar la página de inicio de sesión para su visualización, 
        /// gestionando los mensajes de error, la redirección después del inicio de sesión y la limpieza de cookies externas
        /// </summary>
        /// <param name="returnUrl">Define el URL a la cuál se va a retornar después del inicio de sesión. Por defecto es null</param>
        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        /// <summary>
        /// Método invocado cuando se realiza una solicitud POST para la página de inicio de sesión. 
        /// Si la información brindada es válida, se loguea y se redirige a la página de inicio.
        /// </summary>
        /// <param name="returnUrl">Define el URL a la cuál se va a retornar después del inicio de sesión. Por defecto es null</param>
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Intento de inicio de sesión no válido. Por favor, verifique sus credenciales.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
