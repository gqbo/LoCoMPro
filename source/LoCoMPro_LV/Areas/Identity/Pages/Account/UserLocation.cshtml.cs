// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using LoCoMPro_LV.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LoCoMPro_LV.Data;

namespace LoCoMPro_LV.Areas.Identity.Pages.Account
{
    public class UserInfo
    {
        public UserInfo(string firstName, string lastName, string userName, string email, string password)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.userName = userName;
            this.email = email;
            this.password = password;
        }

        public string firstName { get; set; }
        public string lastName { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public string password { get; set; }

    }

    /// <summary>
    /// Clase que maneja la lógica relacionada con el registro de usuarios en la aplicación web.
    /// Recopila y valida la información que los usuario proporcionan al registrarse en la aplicación web.
    /// </summary>
    public class UserLocationModel : PageModel
    {
        /// <summary>
        /// Gestiona el registro de los usuarios.
        /// </summary>
        private readonly SignInManager<ApplicationUser> _signInManager;
        /// <summary>
        /// Administra a los usuarios de tipo ApplicationUser.
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;
        /// <summary>
        /// Interfaz que proporciona una abstracción para el almacenamiento de usuarios.
        /// </summary>
        private readonly IUserStore<ApplicationUser> _userStore;
        /// <summary>
        ///  Almacena y recupera información relacionada con las direcciones de correo electrónico de los usuarios.
        /// </summary>
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        /// <summary>
        ///  Registra información, advertencias y errores.
        /// </summary>
        private readonly ILogger<RegisterModel> _logger;
        /// <summary>
        ///  Interfaz que se utiliza para enviar correos electrónicos desde la aplicación.
        /// </summary>
        private readonly IEmailSender _emailSender;
        /// <summary>
        ///  Contexto de la base de datos de la aplicación.
        /// </summary>
        private readonly LoComproContext _context;

        /// <summary>
        /// Constructor de la clase RegisterModel.
        /// Se encarga de inicializar los atributos de la clase para el manejo de usuarios.
        /// </summary>
        /// <param name="userManager">Administra a los usuarios de tipo ApplicationUser.</param>
        /// <param name="userStore">Interfaz que proporciona una abstracción para el almacenamiento de usuarios.</param>
        /// <param name="signInManager">Gestiona el registro de los usuarios.</param>
        /// <param name="logger">Registra información, advertencias y errores.</param>
        /// <param name="emailSender">Interfaz que se utiliza para enviar correos electrónicos desde la aplicación.</param>
        /// <param name="context">Contexto de la base de datos de la aplicación.</param>
        public UserLocationModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            LoComproContext context)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }


        /// <summary>
        /// Atributo que almacena el URL a donde se va a redirigir después de registrar un usuario.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Latitud de la tienda seleccionada en la pantalla seleccionar ubicación.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public double Latitude { get; set; }

        /// <summary>
        /// Longitud de la tienda seleccionada en la pantalla seleccionar ubicación.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public double Longitude { get; set; }

        /// <summary>
        /// Nombre del cantón de la tienda obtenido en la pantalla seleccionar ubicación.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string NameCanton { get; set; }

        /// <summary>
        /// Nombre de la provincia obtenido en la pantalla seleccionar ubicación.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string NameProvince { get; set; }

        /// <summary>
        /// Propiedad que almacena una lista de esquemas de autenticación externa disponibles.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public UserInfo userInfo { get; set; }

        /// <summary>
        /// Método invocado cuando se realiza una solicitud GET que prepara la página de registro para su visualización, 
        /// obteniendo los esquemas de autenticación externa disponibles y, opcionalmente, almacenando la URL de retorno para 
        /// redirigir al usuario después de que se registre.
        /// </summary>
        /// <param name="returnUrl">Define el URL a la cuál se va a retornar después de registrar un usuario. Por defecto es null</param>
        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        /// <summary>
        /// Método utilizado para recibir solicitudes POST desde la vista Register, con el objetivo de guardar la información que
        /// el usuario ingresó en el formulario.
        /// </summary>
        public IActionResult OnPostSaveUserInfo(string firstName, string lastName, string userName, string email, string password)
        {
            try
            {
                userInfo = new UserInfo(firstName, lastName, userName, email, password);
                return new OkResult();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await Task.Delay(0);
                return new OkResult();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Método que crea y devuelve una instancia de ApplicationUser con los atributos necesarios para su creación.
        /// </summary>
        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        /// <summary>
        /// Método que obtiene la implementación del almacén de correos electrónicos asociado a un Applicationuser
        /// </summary>
        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}