using System.ComponentModel.DataAnnotations;
using LoCoMPro_LV.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LoCoMPro_LV.Areas.Identity.Pages.Account.Manage
{
    public class ChangeLocationModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;

        public ChangeLocationModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<ChangePasswordModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            [Required(ErrorMessage = "El grado de latitud es necesario")]
            public double Latitude { get; set; }

            [Required(ErrorMessage = "El grado de longitud es necesario")]
            public double Longitude { get; set; }

            [Display(Name = "Provincia")]
            [Required(ErrorMessage = "El nombre de la provincia es obligatorio.")]
            public string NameProvince { get; set; }

            [Display(Name = "Cantón")]
            [Required(ErrorMessage = "El nombre del cantón es obligatorio.")]
            public string NameCanton { get; set; }
        }

        private void Load(ApplicationUser user)
        {
            var latitude = user.Latitude;
            var longitude = user.Longitude;
            var nameProvince = user.NameProvince;
            var nameCanton = user.NameCanton;

            Input = new InputModel
            {
                Latitude = latitude,
                Longitude = longitude,
                NameProvince = nameProvince,
                NameCanton = nameCanton,
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var AuthenticatedUserName = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(AuthenticatedUserName);
            if (user == null)
            {
                return NotFound($"Unable to load user with UserName: '{AuthenticatedUserName}'.");
            }

            Load(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var AuthenticatedUserName = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(AuthenticatedUserName);
            if (user == null)
            {
                return NotFound($"Unable to load user with UserName: '{AuthenticatedUserName}'.");
            }

            var latitude = user.Latitude;
            var longitude = user.Longitude;
            var nameProvince = user.NameProvince;
            var nameCanton = user.NameCanton;

            if (Input.Latitude != latitude)
            {
                user.Latitude = Input.Latitude;
            }

            if (Input.Longitude != longitude)
            {
                user.Longitude = Input.Longitude;
            }

            if (Input.NameProvince != nameProvince)
            {
                user.NameProvince = Input.NameProvince;
            }

            if (Input.NameCanton != nameCanton)
            {
                user.NameCanton = Input.NameCanton;
            }

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Se ha actualizado la ubicación";

            return RedirectToPage();
        }
    }
}
