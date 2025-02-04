﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using LoCoMPro_LV.Utils;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using LoCoMPro_LV.Data;
using System.Data.SqlClient;
using System.Data;

namespace LoCoMPro_LV.Pages.Records
{
    /// <summary>
    /// Página Create de Records para la creación de nuevos registros registros.
    /// </summary>
    public class CreateModel : PageModel
    {
        /// <summary>
        /// Contexto de la base de datos de LoCoMPro.
        /// </summary>
        private readonly LoComproContext _context;
        /// <summary>
        /// Contexto de la base de datos de LoCoMPro sección de registros.
        /// </summary>
        private readonly SignInManager<ApplicationUser> _signInManager;
        /// <summary>
        /// Se utiliza para acceder a las utilidades de la base de datos.
        /// </summary>
        private readonly DatabaseUtils _databaseUtils;

        public CreateModel(LoComproContext context, SignInManager<ApplicationUser> signInManager, DatabaseUtils databaseUtils)
        {
            _context = context;
            _signInManager = signInManager;
            _databaseUtils = databaseUtils;
        }

        /// <summary>
        /// Método invocado cuando se realiza una solicitud GET para crear registros. 
        /// Realiza una serie de llamados a los diferentes métodos encargados de obtener la información de la base de datos.
        /// </summary>
        public async Task OnGetAsync(double latitude, double longitude, string nameStore, string nameProvince, string nameCanton)
        {
            await LoadStoresAsync();
            await LoadProductsAsync();
            await LoadCategoriesAsync();

            Latitude = latitude;
            Longitude = longitude;
            NameStore = nameStore;
            NameProvince = nameProvince;
            NameCanton = nameCanton;
            LoadAuthenticatedUserName();
        }

        /// <summary>
        /// Enlazar los valores de las propiedades en un objeto con los datos provenientes de una solicitud HTTP.
        /// </summary>
        [BindProperty]
        public Record Record { get; set; }

        /// <summary>
        /// Colección de datos donde se almacena los locales que se encuentran en la BD.
        /// </summary>
        public HashSet<string> Stores { get; set; }

        /// <summary>
        /// Lista donde se almacena los productos que se encuentran en la BD.
        /// </summary>
        public List<string> Product { get; set; }

        /// <summary>
        /// Lista seleccionable donde se almacena las categorías que se encuentran en la BD.
        /// </summary>
        public SelectList Categories { get; set; }

        /// <summary>
        /// String donde se almacenar el usuario que se encuentran autenticado.
        /// </summary>
        public string AuthenticatedUserName { get; set; }

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
        /// Nombre de la tienda seleccionada en la pantalla seleccionar ubicación.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string NameStore { get; set; }

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

        [BindProperty(SupportsGet = true)]
        public List<IFormFile> ImageFiles { get; set; }

        /// <summary>
        /// Método que carga los datos ingresados por el usuario a los registros y a las diferentes tablas de la base de datos. 
        /// Realiza una serie de llamados que validan la consistencia de los datos que se desean añadir en la base de datos.
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            await ProcessStore();
            await ProcessProduct();
            await ProcessAssociated();
            await ProcessRecord();
            await ProcessImage();

            var user = await _context.ModeratorUsers
                .FirstOrDefaultAsync(m => m.UserName == Record.NameGenerator);

            if (user == null)
                CallSetModeratorProcedure(Record.NameGenerator);

            return RedirectToPage("../Index");
        }

        /// <summary>
        /// Llama al procedimiento almacenado para designar a un usuario como moderador.
        /// </summary>
        /// <param name="username">El nombre de usuario del usuario que se asignara como moderador.</param>
        private void CallSetModeratorProcedure(string username)
        {
            string connectionString = _databaseUtils.GetConnectionString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SetModerator", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@NameGenerator", username);

                    int rowsAffected = command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// String de validación de datos para Category.
        /// </summary>
        [BindProperty]
        [Required(ErrorMessage = "La categoría es obligatoria")]
        public string SelectCategory { get; set; }

        /// <summary>
        /// Permite almacenar los locales en una colección de datos.
        /// </summary>
        public async Task LoadStoresAsync()
        {
            var stores = await _context.Stores.ToListAsync();
            Stores = new HashSet<string>(stores.Select(store => store.NameStore));
        }

        /// <summary>
        /// Permite obtener y almacenarlos productos en una lista.
        /// </summary>
        public async Task LoadProductsAsync()
        {
            var products = await _context.Products.ToListAsync();
            Product = products.Select(prod => prod.NameProduct).ToList();
        }

        /// <summary>
        /// Permite obtener y almacenar las categorías en una lista.
        /// </summary>
        public async Task LoadCategoriesAsync()
        {
            var categories = await _context.Categories.OrderBy(c => c.NameCategory).ToListAsync();
            Categories = new SelectList(categories, "NameCategory", "NameCategory");

        }

        /// <summary>
        /// Permite obtener en un String el nombre del usuario autentificado.
        /// </summary>
        private void LoadAuthenticatedUserName()
        {
            if (User.Identity.IsAuthenticated)
            {
                AuthenticatedUserName = User.Identity.Name;
            }
        }

        /// <summary>
        /// Verifica si es una nueva tienda, en caso de que no lo sea no la almacena.
        /// </summary>
        private async Task ProcessStore()
        {
            var existingStore = await _context.Stores.FirstOrDefaultAsync(s =>
            s.NameStore == NameStore);

            var existingEqualStore = await _context.Stores.FirstOrDefaultAsync(s =>
            s.NameStore == NameStore && s.Latitude == Latitude && s.Longitude == Longitude);
            if (existingEqualStore != null)
            {
                Record.Store = existingStore;
                Record.NameStore = NameStore;
                Record.Longitude = Longitude;
                Record.Latitude = Latitude;
            }
            else if (existingStore != null)
            {
                var distance = Geolocation.CalculateDistance(existingStore.Latitude, existingStore.Longitude, Latitude, Longitude);
                if (distance <= 2000)
                {
                    Record.Store = existingStore;
                    Record.NameStore = NameStore;
                    Record.Longitude = Longitude;
                    Record.Latitude = Latitude;
                }
                else
                {
                    var newStore = new Store
                    {
                        NameStore = NameStore,
                        NameProvince = NameProvince,
                        NameCanton = NameCanton,
                        Latitude = Latitude,
                        Longitude = Longitude,
                    };
                    _context.Stores.Add(newStore);
                    await _context.SaveChangesAsync();
                    Record.NameStore = newStore.NameStore;
                    Record.Longitude = newStore.Longitude;
                    Record.Latitude = newStore.Latitude;
                }
            }
            else
            {
                var newStore = new Store
                {
                    NameStore = NameStore,
                    NameProvince = NameProvince,
                    NameCanton = NameCanton,
                    Latitude = Latitude,
                    Longitude = Longitude,
                };
                _context.Stores.Add(newStore);
                await _context.SaveChangesAsync();
                Record.NameStore = newStore.NameStore;
                Record.Longitude = newStore.Longitude;
                Record.Latitude = newStore.Latitude;
            }  
        }

        /// <summary>
        /// Valida que no se repita el producto a almacenar en la BD.
        /// </summary>
        private async Task ProcessProduct()
        {
            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.NameProduct == Record.NameProduct);

            if (existingProduct != null)
            {
                Record.Product = existingProduct;
            }
            else
            {
                var newProduct = new Product
                {
                    NameProduct = Record.NameProduct
                };
                _context.Products.Add(newProduct);
                await _context.SaveChangesAsync();
                Record.Product = newProduct;
            }
        }


        /// <summary>
        /// Valida que no repita un asociación entre categoría y producto a la hora de almacenarlo en la BD.
        /// </summary>
        private async Task ProcessAssociated()
        {
            var existingAssociated = await _context.Associated.FirstOrDefaultAsync(a =>
                a.NameProduct == Record.NameProduct &&
                a.NameCategory == SelectCategory);

            if (existingAssociated == null)
            {
                var newAssociated = new Associated
                {
                    NameProduct = Record.NameProduct,
                    NameCategory = SelectCategory
                };
                _context.Associated.Add(newAssociated);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Método que verifica la hora actual para almacenarla en la BD.
        /// </summary>
        private static DateTime GetCurrentDateTime()
        {
            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return DateTime.ParseExact(currentDateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Almacena en el contexto de la base de datos el registro, con la información recolectada.
        /// </summary>
        private async Task ProcessRecord()
        {
            Record.RecordDate = GetCurrentDateTime();
            _context.Records.Add(Record);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Procesa los datos del archivo para almacenar las imágenes del registro dentro de la base de datos.
        /// </summary>
        private async Task ProcessImage()
        {
            foreach (var imageFile in ImageFiles)
            {
                var fileName = imageFile.FileName;
                var data = await GetBytesFromImageAsync(imageFile);
                var image = new Image
                {
                    NameGenerator = Record.NameGenerator,
                    RecordDate = Record.RecordDate,
                    NameImage = fileName,
                    DataImage = data
                };
                _context.Images.Add(image);
            }
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Obtiene la cantidad de bytes que contiene el archivo IFormFile
        /// </summary>
        /// <param name="imageFile"> Archivo relacionado a la imagen.</param>
        private async Task<byte[]> GetBytesFromImageAsync(IFormFile imageFile)
        {
            using (var stream = new MemoryStream())
            {
                await imageFile.CopyToAsync(stream);
                return stream.ToArray();
            }
        }
    }
}
