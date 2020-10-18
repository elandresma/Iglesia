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
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Iglesia.Prism.ViewModels
{
    public class AddMeetingPageViewModel : ViewModelBase
    {
        private DateTime _date;
        private bool _isRunning;
        private readonly IApiService _apiService;
        private readonly INavigationService _navigationService;
        private DelegateCommand _addmeetingcommand;

        public AddMeetingPageViewModel(IApiService apiService,INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.AddMeeting;
        }

        public DelegateCommand AddMeetingCommand => _addmeetingcommand ??
    (_addmeetingcommand = new DelegateCommand(CreateMeetingAsync));
        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        [Obsolete]
        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        public Command DateChosen
        {
            get
            {
                return new Command((obj) =>
                {
                    System.Diagnostics.Debug.WriteLine(obj as DateTime?);
                });
            }
        }

        private async void CreateMeetingAsync()
        {
            IsRunning = true;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }
            MeetingRequest request = new MeetingRequest
            {
                Date = Date,
                ChurchId = Int32.Parse(JsonConvert.DeserializeObject<String>(Settings.Church))

            };
            string url = App.Current.Resources["UrlAPI"].ToString();
            
            Response response = await _apiService.CreateMeetingAsync<MeetingResponse>(
                url,
                "/api",
                "/Meeting/Create", request);
            IsRunning = false;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            await App.Current.MainPage.DisplayAlert(Languages.Ok, Languages.AddMeetingMessage, Languages.Accept);
            await _navigationService.GoBackAsync();

        }
    }
}

