using Microsoft.AspNetCore.Mvc.RazorPages;
using LoCoMPro_LV.Data;
using LoCoMPro_LV.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using LoCoMPro_LV.Areas.Identity.Pages.Account;
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

            default:
                return new LoCoMPro_LV.Pages.IndexModel(dbContext);
        }
    }
}
