using Microsoft.AspNetCore.Mvc.RazorPages;
using LoCoMPro_LV.Data;
using LoCoMPro_LV.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using LoCoMPro_LV.Areas.Identity.Pages.Account;
using LoCoMPro_LV.Pages.Stores;

using Microsoft.Extensions.Logging;

public class TestUtils
{
    protected UserManager<ApplicationUser> UserManager { get; set; }
    public TestUtils()
    {
        var userStore = new Mock<IUserStore<ApplicationUser>>();
        UserManager = new UserManager<ApplicationUser>(userStore.Object, null!, null!, null!, null!, null!, null!, null!, null!);
    }

    protected PageModel ConfigurePageModel(string namePage)
    {
        var options = new DbContextOptionsBuilder<LoComproContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
            .Options;

        var dbContext = new LoComproContext(options);

        var LoggerRegister = new Mock<ILogger<RegisterModel>>();

        var UserStore = new Mock<IUserStore<ApplicationUser>>();

        var HttpContextAccessor = new Mock<IHttpContextAccessor>();

        UserStore.Setup(s => s.FindByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string userName, CancellationToken cancellationToken) =>
            {
                return new ApplicationUser { UserName = userName };
            });

        var UserManager = new Mock<UserManager<ApplicationUser>>(UserStore.Object, null, null
            , null, null, null, null, null, null);
        UserManager.Object.UserValidators.Add(new UserValidator<ApplicationUser>());
        UserManager.Object.PasswordValidators.Add(new PasswordValidator<ApplicationUser>());
        UserManager.Setup(u => u.SupportsUserEmail).Returns(true);

        var UserClaimsPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();

        var SignInManager = new Mock<SignInManager<ApplicationUser>>(
            UserManager.Object,
            HttpContextAccessor.Object,
            UserClaimsPrincipalFactory.Object, null, null, null, null
        );

        return GetPageModel(dbContext, UserManager, SignInManager,
          UserStore , LoggerRegister, namePage);
    }

    protected PageModel GetPageModel(LoComproContext dbContext
        , Mock<UserManager<ApplicationUser>> UserManager
        , Mock<SignInManager<ApplicationUser>> SignInManager
        , Mock<IUserStore<ApplicationUser>> UserStore, Mock<ILogger<RegisterModel>> LoggerRegister
        , string namePage)
    {
        switch (namePage)
        {
            case "register_page":
                return new RegisterModel(UserManager.Object, UserStore.Object, SignInManager.Object, LoggerRegister.Object, dbContext);
            case "createStore_page":
                return new CreateStoreModel(dbContext);
            default:
                return new LoCoMPro_LV.Pages.IndexModel(dbContext);
        }
    }

    protected List<Record> CreateRegisters()
    {
        var user = new ApplicationUser
        {
            UserName = "anne",
            FirstName = "Anne",
            LastName = "Hathaway",
            Latitude = 10.009,
            Longitude = -84.1211,
            NameProvince = "Heredia",
            NameCanton = "Barva",
            NormalizedUserName = "ANNE",
            Email = "anne@gmail.com",
            NormalizedEmail = "ANNE@GMAIL.COM",
            EmailConfirmed = false,
            PasswordHash = "AQAAAAIAAYagAAAAEJmsnmp+Rm7C6VMHu1s21eBFFButNJUpJ6E6yV5OnERn6c2Hv7KutoQrAaUPyez1lQ==",
            SecurityStamp = "5NFRN2JU2Z7T7WPCJZ4LY4TA45YHLSXW",
            ConcurrencyStamp = "cd77e5d0-740a-43f8-9ad4-3d8633ec6d01",
            LockoutEnabled = true,
            AccessFailedCount = 0
        };

        var generatorUser = new GeneratorUser
        {
            UserName = "anne",
            ApplicationUser = user
        };

        var product = new Product
        {
            NameProduct = "Apple Iphone 11 64gb"
        };

        var province = new Province
        {
            NameProvince = "San José"
        };

        var canton = new Canton
        {
            NameCanton = "Tibás",
            Province = province,
            NameProvince = "San José"
        };

        var store = new Store
        {
            NameStore = "Ishop",
            Canton = canton,
            NameProvince = "San José",
            NameCanton = "Tibás",
            Latitude = 9.9516,
            Longitude = -84.0990
        };

        Random random = new Random();

        var result = new List<Record>();

        for (int i = 0; i < 5; ++i)
        {   
            int year = random.Next(2022, 2024); 
            int month = random.Next(1, 13); 
            int day = random.Next(1, 29); 

            DateTime randomDate = new DateTime(year, month, day);

            result.Add(new Record()
            {
                NameGenerator = "anne",
                GeneratorUser = generatorUser,
                RecordDate = randomDate,
                Price = random.Next(743, 1369),
                NameStore = "Ishop",
                Latitude = 9.9516,
                Longitude = -84.0990,
                NameProduct = "Apple Iphone 11 64gb",
                Store = store,
                Product = product,
                Description = "Test comment"
            });
        }
        return result;
    }
}
