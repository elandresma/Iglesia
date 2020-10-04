using Iglesia.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using Iglesia.Prism.Resources;

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

    }


}
