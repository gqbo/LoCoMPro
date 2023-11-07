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
using LoCoMPro_LV.Pages.Stores;

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
            case "createStore_page":
                return new CreateStoreModel(dbContext);
            default:
                return new LoCoMPro_LV.Pages.IndexModel(dbContext);
        }
    }
}
