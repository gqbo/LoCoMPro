using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using LoCoMPro_LV.Areas.Identity.Pages.Account;
using LoCoMPro_LV.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace nunit_tests
{
    [TestFixture]
    public class UnitTests_CreateReport : TestUtils
    {
        // Test by Cristopher Hernandez Calderon - C13632
        [Test]
        public async Task OnGetAsync_ValidFormat_DoesNotThrowException()
        {
            var model = ConfigurePageModel("createReports_page") as LoCoMPro_LV.Pages.Reports.CreateModel;
            model.NameGenerator = "brad";
            model.RecordDate = DateTime.Parse("2022-1-18");

            Assert.DoesNotThrow(() => model.OnGetAsync());
        }
    }
}