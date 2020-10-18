using Iglesia.Common.Helpers;
using Iglesia.Common.Requests;
using Iglesia.Common.Responses;
using Iglesia.Common.Services;
using Iglesia.Prism.Helpers;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;

namespace Iglesia.Prism.ViewModels
{
    public class MyAssistancesPageViewModel : ViewModelBase
    {

        private List<AssistancesResponse> _assistances;
        private readonly IApiService _apiService;
        private bool _isRunning;

        public MyAssistancesPageViewModel(INavigationService navigationService, IApiService apiService)
           : base(navigationService)
        {
            _apiService = apiService;
            Title = Languages.Assistances;
            LoadMyAssistances();
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }
        public List<AssistancesResponse> Assistances
        {
            get => _assistances;
            set => SetProperty(ref _assistances, value);
        }
        private async void LoadMyAssistances()
        {

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }
            EmailRequest request = new EmailRequest
            {
                Email = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token).User.Email.ToString()
            };

            string url = App.Current.Resources["UrlAPI"].ToString();

            Response response = await _apiService.GetMyassistancesAsync<AssistancesResponse>(
                url,
                "/api",
                "/Meeting/GetAssistances", request);
            IsRunning = false;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            _assistances = (List<AssistancesResponse>)response.Result;
            Assistances = new List<AssistancesResponse>(_assistances);

        }
    }
}
