using Microsoft.AspNetCore.Mvc.RazorPages;
using LoCoMPro_LV.Data;
using LoCoMPro_LV.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using LoCoMPro_LV.Areas.Identity.Pages.Account;
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

        var mockLogger = new Mock<ILogger<RegisterModel>>();

        var mockUserStore = new Mock<IUserStore<ApplicationUser>>();

        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

        mockUserStore.Setup(s => s.FindByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string userName, CancellationToken cancellationToken) =>
            {
                return new ApplicationUser { UserName = userName };
            });

        var mockUserManager = new Mock<UserManager<ApplicationUser>>(mockUserStore.Object, null, null
            , null, null, null, null, null, null);
        mockUserManager.Object.UserValidators.Add(new UserValidator<ApplicationUser>());
        mockUserManager.Object.PasswordValidators.Add(new PasswordValidator<ApplicationUser>());

        var mockUserClaimsPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();

        var mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
            mockUserManager.Object,
            mockHttpContextAccessor.Object,
            mockUserClaimsPrincipalFactory.Object, null, null, null, null
        );

        return ConfigurePageModel(dbContext, mockUserManager, mockSignInManager
            , mockUserStore, mockLogger, namePage);
    }

    protected PageModel ConfigurePageModel(LoComproContext dbContext
        , Mock<UserManager<ApplicationUser>> mockUserManager
        , Mock<SignInManager<ApplicationUser>> mockSignInManager
        , Mock<IUserStore<ApplicationUser>> mockUserStore, Mock<ILogger<RegisterModel>> mockLogger
        , string namePage)
    {
        switch (namePage)
        {
            case "register_page":
                return new RegisterModel(mockUserManager.Object, mockUserStore.Object, mockSignInManager.Object, mockLogger.Object, dbContext);

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
            NameProvince = "San Jos�"
        };

        var canton = new Canton
        {
            NameCanton = "Tib�s",
            Province = province,
            NameProvince = "San Jos�"
        };

        var store = new Store
        {
            NameStore = "Ishop",
            Canton = canton,
            NameProvince = "San Jos�",
            NameCanton = "Tib�s",
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
