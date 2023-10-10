// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using LoCoMPro_LV.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace LoCoMPro_LV.Areas.Identity.Pages.Account
{
    /// <summary>
    /// Clase que maneja la lógica relacionada con el logout en la aplicación web.
    /// Cierra la sesión del usuario que se encuentra logueado.
    /// </summary>
    public class LogoutModel : PageModel
    {
        /// <summary>
        /// Gestiona el logout de los usuarios.
        /// </summary>
        private readonly SignInManager<ApplicationUser> _signInManager;
        /// <summary>
        ///  Registra información, advertencias y errores.
        /// </summary>
        private readonly ILogger<LogoutModel> _logger;

        /// <summary>
        /// Constructor de la clase LogoutModel.
        /// Se encarga de inicializar los atributos de la clase para el manejo de usuarios.
        /// </summary>
        /// <param name="signInManager">Gestiona el logout de los usuarios.</param>
        /// <param name="logger">Registra información, advertencias y errores.</param>
        public LogoutModel(SignInManager<ApplicationUser> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        /// <summary>
        /// Método invocado cuando se realiza una solicitud POST para la página de logout.
        /// Cierra la sesión del usuario y lo retorna a la página actual o a la página de inicio.
        /// </summary>
        /// <param name="returnUrl">Define el URL a la cuál se va a retornar después de cerrar sesión de un usuario. Por defecto es null</param>
        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage();
            }
        }
    }
}
