﻿using Iglesia.Common.Helpers;
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

        public static string Register => Resource.Register;


        public static string Document => Resource.Document;

        public static string DocumentError => Resource.DocumentError;

        public static string DocumentPlaceHolder => Resource.DocumentPlaceHolder;

        public static string FirstName => Resource.FirstName;

        public static string FirstNameError => Resource.FirstNameError;

        public static string FirstNamePlaceHolder => Resource.FirstNamePlaceHolder;

        public static string LastName => Resource.LastName;

        public static string LastNameError => Resource.LastNameError;

        public static string LastNamePlaceHolder => Resource.LastNamePlaceHolder;

        public static string Address => Resource.Address;

        public static string AddressError => Resource.AddressError;

        public static string AddressPlaceHolder => Resource.AddressPlaceHolder;

        public static string PhoneError => Resource.PhoneError;

        public static string PhonePlaceHolder => Resource.PhonePlaceHolder;

        public static string ChurchError => Resource.Church;

        public static string ChurchPlaceHolder => Resource.ChurchPlaceHolder;

        public static string District => Resource.District;

        public static string DistrictError => Resource.DistrictError;

        public static string DistrictPlaceHolder => Resource.DistrictPlaceHolder;

        public static string Region => Resource.Region;

        public static string RegionError => Resource.RegionError;

        public static string RegionPlaceHolder => Resource.RegionPlaceHolder;

        public static string PasswordConfirm => Resource.PasswordConfirm;

        public static string PasswordConfirmError1 => Resource.PasswordConfirmError1;

        public static string PasswordConfirmError2 => Resource.PasswordConfirmError2;

        public static string PasswordConfirmPlaceHolder => Resource.PasswordConfirmPlaceHolder;

        public static string Error001 => Resource.Error001;


        public static string Error003 => Resource.Error003;

        public static string Error004 => Resource.Error004;

        public static string Ok => Resource.Ok;

        public static string RegisterMessage => Resource.RegisterMessage;

        public static string PictureSource => Resource.PictureSource;

        public static string Cancel => Resource.Cancel;

        public static string FromCamera => Resource.FromCamera;

        public static string FromGallery => Resource.FromGallery;

        public static string NoCameraSupported => Resource.NoCameraSupported;

        public static string NoGallerySupported => Resource.NoGallerySupported;

        public static string RecoverPassword => Resource.RecoverPassword;

        public static string RecoverPasswordMessage => Resource.RecoverPasswordMessage;

        public static string ChangePassword => Resource.ChangePassword;

        public static string ChangeUserMessage => Resource.ChangeUserMessage;

        public static string ProfessionError => Resource.ProfessionError;
        public static string ProfessionPlaceHolder => Resource.ProfessionPlaceHolder;

        public static string Profession => Resource.Profession;

        public static string ConfirmNewPassword => Resource.ConfirmNewPassword;

        public static string ConfirmNewPasswordError1 => Resource.ConfirmNewPasswordError1;

        public static string ConfirmNewPasswordError2 => Resource.ConfirmNewPasswordError2;

        public static string ConfirmNewPasswordPlaceHolder => Resource.ConfirmNewPasswordPlaceHolder;

        public static string CurrentPassword => Resource.CurrentPassword;

        public static string CurrentPasswordError => Resource.CurrentPasswordError;

        public static string CurrentPasswordPlaceHolder => Resource.CurrentPasswordPlaceHolder;

        public static string NewPassword => Resource.NewPassword;

        public static string NewPasswordError => Resource.NewPasswordError;

        public static string NewPasswordPlaceHolder => Resource.NewPasswordPlaceHolder;

        public static string Error005 => Resource.Error005;
        public static string Save => Resource.Save;

        public static string ChangePassworrdMessage => Resource.ChangePassworrdMessage;

        public static string AddMeeting => Resource.AddMeeting;
        public static string AddMeetingMessage => Resource.AddMeetingMessage;
        public static string SelectDate => Resource.SelectDate;
        public static string Update => Resource.Update;
        public static string UpdateAssistancesMessage => Resource.UpdateAssistancesMessage;
        public static string AddUsers => Resource.AddUsers;
        public static string IsPresent => Resource.IsPresent;
        public static string DateMeeting => Resource.DateMeeting;
        public static string SearchAUser => Resource.SearchAUser;
        public static string Members => Resource.Members;
    }


}
