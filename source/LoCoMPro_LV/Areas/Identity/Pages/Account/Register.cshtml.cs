// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using LoCoMPro_LV.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Mvc.RazorPages;
using LoCoMPro_LV.Data;

namespace LoCoMPro_LV.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
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
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

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

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                user.Email = Input.Email;
                user.UserName = Input.UserName;
                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;


                await _userStore.SetUserNameAsync(user, user.UserName, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, user.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    GeneratorUser generatorUser = new GeneratorUser { UserName = user.UserName, ApplicationUser = user };
                    _context.GeneratorUsers.Add(generatorUser);
                    await _context.SaveChangesAsync();
                    return LocalRedirect(returnUrl);

                }
                foreach (var error in result.Errors)
                {
                    //ModelState.AddModelError(string.Empty, error.Description);
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

            // If we got this far, something failed, redisplay form
            return Page();
        }

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