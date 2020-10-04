using DryIoc;
using Iglesia.Common.Requests;
using Iglesia.Common.Responses;
using Iglesia.Common.Services;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Essentials;

namespace Iglesia.Prism.ViewModels
{
    public class UsersPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private ObservableCollection<UserResponse> _user;

        public UsersPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Users";
            GetUsersByChurch();
        }

        public ObservableCollection<UserResponse> Users
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        private async void GetUsersByChurch()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Check the internet connection.", "Accept");
                return;
            }

            string url = App.Current.Resources["UrlAPI"].ToString();
            EmailRequest teacherEmail = new EmailRequest
            {
                Email = "Teacher2@yopmail.com"
            };
            Response response = await _apiService.GetUsersAsync<UserResponse>(
                url,
                "/api",
                "/Churches/GetUsersByChurch", teacherEmail);

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            List<UserResponse> myUsers = (List<UserResponse>)response.Result;
            Users = new ObservableCollection<UserResponse>(myUsers);
        }
    }

}
