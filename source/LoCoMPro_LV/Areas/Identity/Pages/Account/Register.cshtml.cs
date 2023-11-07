using System.ComponentModel.DataAnnotations;
using LoCoMPro_LV.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LoCoMPro_LV.Data;

namespace LoCoMPro_LV.Areas.Identity.Pages.Account
{
    /// <summary>
    /// Clase que maneja la lógica relacionada con el registro de usuarios en la aplicación web.
    /// Recopila y valida la información que los usuario proporcionan al registrarse en la aplicación web.
    /// </summary>
    public class RegisterModel : PageModel
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

        public RegisterModel(
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

        [BindProperty]
        /// <summary>
        /// Modelo que se utiliza para recopilar los datos que el usuario ingresa en el formulario de registro de usuarios.
        /// </summary>
        public InputModel Input { get; set; }

        /// <summary>
        /// Atributo que almacena el URL a donde se va a redirigir después de registrar un usuario.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Latitud obtenida del mapa interactivo.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public double Latitude { get; set; }

        /// <summary>
        /// Longitud obtenida del mapa interactivo.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public double Longitude { get; set; }

        /// <summary>
        /// Nombre del cantón obtenido del mapa interactivo.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string NameCanton { get; set; }

        /// <summary>
        /// Nombre de la provincia obtenido del mapa interactivo.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string NameProvince { get; set; }

        /// <summary>
        /// Propiedad que almacena una lista de esquemas de autenticación externa disponibles.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        /// Clase que se utiliza para definir los datos que se recopilan en el formulario de registro de usuarios y sus restricciones.
        /// </summary>
        public class InputModel
        {

            [PersonalData]
            [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
            [EmailAddress(ErrorMessage = "La dirección de correo electrónico es inválida.")]
            [Display(Name = "Correo electrónico")]
            public string Email { get; set; }

            [PersonalData]
            [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
            [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre de usuario debe tener entre 2 y 50 caracteres.")]
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "El nombre de usuario no es válido. Únicamente se admiten letras minúsculas, mayúsculas y números.")]
            [Display(Name = "Usuario")]
            public string UserName { get; set; }

            [PersonalData]
            [Required(ErrorMessage = "La contraseña es obligatoria.")]
            [StringLength(50, MinimumLength = 2, ErrorMessage = "La contraseña debe tener entre 2 y 50 caracteres.")]
            [DataType(DataType.Password)]
            [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "La contraseña debe contener al menos una letra mayúscula y al menos un número.")]
            [Display(Name = "Contraseña")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar contraseña")]
            [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
            public string ConfirmPassword { get; set; }

            [PersonalData]
            [Required(ErrorMessage = "El nombre es obligatorio.")]
            [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres.")]
            [RegularExpression(@"^[a-zA-ZáéíóúñÑÁÉÍÓÚ]+$", ErrorMessage = "El nombre debe contener solo letras (mayúsculas o minúsculas).")]
            [Display(Name = "Nombre")]
            public string FirstName { get; set; }

            [PersonalData]
            [Required(ErrorMessage = "El apellido es obligatorio.")]
            [StringLength(50, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 50 caracteres.")]
            [RegularExpression(@"^[a-zA-ZáéíóúñÑÁÉÍÓÚ]+$", ErrorMessage = "El apellido debe contener solo letras (mayúsculas o minúsculas).")]
            [Display(Name = "Apellido")]
            public string LastName { get; set; }
        }

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
        /// Método invocado cuando se realiza una solicitud POST para la página de registro de usuarios. 
        /// Crea un nuevo usuario desde cero con la información brindada en el formulario de registro de usuarios.
        /// Si la información brindada es válida, se crea el usuario, se loguea y se redirige a la página de inicio.
        /// </summary>
        /// <param name="returnUrl">Define el URL a la cuál se va a retornar después de registrar un usuario. Por defecto es null</param>
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (NameCanton == "N/A" || NameProvince == "N/A")
            {
                ModelState.AddModelError(string.Empty, "Seleccione una ubicación correcta.");
                return Page();
            }

            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = CreateUser();

                user.Email = Input.Email;
                user.UserName = Input.UserName;
                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
                user.Longitude = Longitude;
                user.Latitude = Latitude;
                user.NameCanton = NameCanton;
                user.NameProvince = NameProvince;

                var result = await RegisterUserAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    return await HandleSuccessfulRegistrationAsync(user, returnUrl);
                }

                HandlePasswordErrors(result);
            }

            return Page();
        }


        /// <summary>
        /// Método invocado para registrar el nombre de usuario y correo electrónico de un usuario, además crea un nuevo usuario.
        /// </summary>
        /// <param name="user">Usuario utilizado para agregarle información como UserName y Email</param>
        /// <param name="password">Contraseña utilizada para crear un nuevo usuario</param>
        private async Task<IdentityResult> RegisterUserAsync(ApplicationUser user, string password)
        {
            await _userStore.SetUserNameAsync(user, user.UserName, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, user.Email, CancellationToken.None);

            return await _userManager.CreateAsync(user, password);
        }

        /// <summary>
        /// Método utilizado para manejar un registro de un usuario exitoso.
        /// <param name="user">Usuario utilizado para agregarle información como UserName y Email</param>
        /// <param name="returnUrl">URL al cuál se va a redirigir después de registrar un usuario</param>
        private async Task<IActionResult> HandleSuccessfulRegistrationAsync(ApplicationUser user, string returnUrl)
        {
            _logger.LogInformation("User created a new account with password.");
            await _signInManager.SignInAsync(user, isPersistent: false);
            GeneratorUser generatorUser = new GeneratorUser { UserName = user.UserName, ApplicationUser = user };
            _context.GeneratorUsers.Add(generatorUser);
            await _context.SaveChangesAsync();
            return LocalRedirect(returnUrl);
        }

        /// <summary>
        /// Método utilizado para manejar los errores relacionados con la contraseña.
        /// <param name="result">Resultado del registro de un usuario para identificar errores relacionados con la contraseña</param>
        private void HandlePasswordErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                if (error.Code == "PasswordRequiresDigit")
                {
                    ModelState.AddModelError(string.Empty, "La contraseña debe contener al menos un número.");
                }
                else if (error.Code == "PasswordTooShort")
                {
                    ModelState.AddModelError(string.Empty, "La contraseña debe tener una longitud mínima de 6 caracteres.");
                }
                else if (error.Code == "PasswordRequiresUniqueChars")
                {
                    ModelState.AddModelError(string.Empty, "La contraseña debe contener al menos un carácter único.");
                }
                else if (!Input.Password.Any(char.IsUpper))
                {
                    ModelState.AddModelError(string.Empty, "La contraseña debe contener al menos un carácter en mayúscula.");
                }
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