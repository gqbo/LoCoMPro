using Microsoft.AspNetCore.Mvc.Rendering;

namespace  LoCoMPro_LV.Areas.Identity.Pages.Account.Manage
{
    /// <summary>
    /// Clase que proporciona métodos para gestionar las páginas relacionadas con la cuenta del usuario.
    /// </summary>
    public static class ManageNavPages
    {
        /// <summary>
        /// Obtiene el nombre de la página "Index".
        /// </summary>
        public static string Index => "Index";

        /// <summary>
        /// Obtiene el nombre de la página "Email".
        /// </summary>
        public static string Email => "Email";

        /// <summary>
        /// Obtiene el nombre de la página "ChangePassword".
        /// </summary>
        public static string ChangePassword => "ChangePassword";

        /// <summary>
        /// Obtiene el nombre de la página "ChangeLocation".
        /// </summary>
        public static string ChangeLocation => "ChangeLocation";

        /// <summary>
        /// Determina si la página "Index" está activa en función de la información proporcionada en viewContext
        /// </summary>
        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        /// <summary>
        /// Determina si la página "Email" está activa en función de la información proporcionada en viewContext
        /// </summary>
        public static string EmailNavClass(ViewContext viewContext) => PageNavClass(viewContext, Email);

        /// <summary>
        /// Determina si la página "ChangePassword" está activa en función de la información proporcionada en viewContext
        /// </summary>
        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);

        /// <summary>
        /// Determina si la página "ChangeLocation" está activa en función de la información proporcionada en viewContext
        /// </summary>
        public static string ChangeLocationNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangeLocation);

        /// <summary>
        /// Indica si una página especificada está activa.
        /// </summary>
        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
