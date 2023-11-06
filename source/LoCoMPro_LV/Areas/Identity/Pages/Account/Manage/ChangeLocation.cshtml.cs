using System.ComponentModel.DataAnnotations;
using LoCoMPro_LV.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LoCoMPro_LV.Areas.Identity.Pages.Account.Manage
{
    public class ChangeLocationModel : PageModel
    {
        /// <summary>
        /// Administra a los usuarios de tipo ApplicationUser.
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;
        /// <summary>
        /// Gestiona el refresh para el inicio de sesión de los usuarios.
        /// </summary>
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ChangeLocationModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Modelo que se utiliza para recopilar los datos que el usuario ingresa a la hora de gestionar su cuenta.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///  Se utiliza para informar al usuario del status de sus cambios.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        /// Clase que se utiliza para definir los datos que se recopilan a la hora de gestionar una cuenta.
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

        /// <summary>
        /// Método utilizado para cargar en el InputModel los datos actuales de un usuario.
        /// </summary>
        /// <param name="user">Usuario actual que desea gestionar su cuenta</param>
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

        /// <summary>
        /// Método invocado cuando se realiza una solicitud GET que prepara la página de gestionar una cuenta relacionado la ubicación.
        /// </summary>
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

        /// <summary>
        /// Método invocado cuando se realiza una solicitud POST que modifica los valores actuales de un usuario con 
        /// los valores ingresados en el formulario.
        /// </summary>
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

            var latitudeBefore = user.Latitude;
            var longitudeBefore = user.Longitude;
            var nameProvinceBefore = user.NameProvince;
            var nameCantonBefore = user.NameCanton;

            if (Input.Latitude != latitudeBefore)
            {
                user.Latitude = Input.Latitude;
            }

            if (Input.Longitude != longitudeBefore)
            {
                user.Longitude = Input.Longitude;
            }

            if (Input.NameProvince != nameProvinceBefore)
            {
                user.NameProvince = Input.NameProvince;
            }

            if (Input.NameCanton != nameCantonBefore)
            {
                user.NameCanton = Input.NameCanton;
            }

            if (Input.Latitude != latitudeBefore || Input.Longitude != longitudeBefore || Input.NameProvince != nameProvinceBefore || Input.NameCanton != nameCantonBefore)
            {
                await _userManager.UpdateAsync(user);
                await _signInManager.RefreshSignInAsync(user);
                StatusMessage = "Se ha actualizado la ubicación";
            }

            return RedirectToPage();
        }
    }
}
