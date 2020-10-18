using Prism;
using Prism.Ioc;
using Iglesia.Prism.ViewModels;
using Iglesia.Prism.Views;
using Xamarin.Essentials.Interfaces;
using Xamarin.Essentials.Implementation;
using Xamarin.Forms;
using Iglesia.Common.Services;
using Syncfusion.Licensing;
using Iglesia.Prism.Helpers;
using Iglesia.Common.Helpers;

namespace Iglesia.Prism
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            SyncfusionLicenseProvider.RegisterLicense("MzM3MDk4QDMxMzgyZTMzMmUzME1VbjB6VkFlcjN1cnJvL2Q3VExVc0JnY21XZm96QTIxbGlsb001SWlmV0U9");
            InitializeComponent();

            await NavigationService.NavigateAsync($"NavigationPage/{nameof(LoginPage)}");
        }


        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.Register<IRegexHelper, RegexHelper>();
            containerRegistry.Register<IFilesHelper, FilesHelper>();


            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<UsersPage, UsersPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<ChurchMasterDetailPage, ChurchMasterDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<ShowMeetingsPage, ShowMeetingsPageViewModel>();
            containerRegistry.RegisterForNavigation<ModifyUserPage, ModifyUserPageViewModel>();
            containerRegistry.RegisterForNavigation<ShowAssistancesPage, ShowAssistancesPageViewModel>();
            containerRegistry.RegisterForNavigation<RegisterPage, RegisterPageViewModel>();
            containerRegistry.RegisterForNavigation<RecoverPasswordPage, RecoverPasswordPageViewModel>();
            containerRegistry.RegisterForNavigation<ChangePasswordPage, ChangePasswordPageViewModel>();
            containerRegistry.RegisterForNavigation<AddMeetingPage, AddMeetingPageViewModel>();
            containerRegistry.RegisterForNavigation<MyAssistancesPage, MyAssistancesPageViewModel>();
            containerRegistry.RegisterForNavigation<AddUsersPage, AddUsersPageViewModel>();
        }
    }
}
