using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoCoMPro_LV.Models
{
    public class ApplicationUser : IdentityUser
    {

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string FirstName { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; }

        public  string NameProvince { get; set; }

        public  string NameCanton { get; set; }

        public Canton Canton { get; set; }
    }
}
