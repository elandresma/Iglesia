using Iglesia.Common.Helpers;
using Iglesia.Prism.Resources;
using System.Globalization;
using Xamarin.Forms;

namespace Iglesia.Prism.Helpers
{
    public static class Languages
    {
        static Languages()
        {
            CultureInfo ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            Culture = ci.Name;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Culture { get; set; }

        public static string Accept => Resource.Accept;

        public static string ConnectionError => Resource.ConnectionError;

        public static string Error => Resource.Error;
        public static string Name => Resource.Name;
        public static string Adress => Resource.Adress;
        public static string Loading => Resource.Loading;
        public static string Church => Resource.Church;
        public static string Email => Resource.Email;
        public static string Phone => Resource.Phone;
        public static string Users => Resource.Users;
        public static string Login => Resource.Login;
        public static string EmailError => Resource.EmailError;

        public static string EmailPlaceHolder => Resource.EmailPlaceHolder;

        public static string Password => Resource.Password;

        public static string PasswordError => Resource.PasswordError;

        public static string PasswordPlaceHolder => Resource.PasswordPlaceHolder;

        public static string ForgotPassword => Resource.ForgotPassword;

        public static string LoginError => Resource.LoginError;

        public static string Logout => Resource.Logout;

        public static string ShowUsersByChurch => Resource.ShowUsersByChurch;

        public static string ModifyUser => Resource.ModifyUser;

        public static string Assistances => Resource.Assistances;


        public static string Meetings => Resource.Meetings;



    }


}
