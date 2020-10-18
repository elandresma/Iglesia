using Iglesia.Common.Helpers;
using Iglesia.Common.Requests;
using Iglesia.Common.Responses;
using Iglesia.Common.Services;
using Iglesia.Prism.Helpers;
using Iglesia.Prism.ItemViewModels;
using Iglesia.Prism.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;

namespace Iglesia.Prism.ViewModels
{
    public class ShowMeetingsPageViewModel :ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private ObservableCollection<MeetingItemViewModel> _meetings;
        private List<MeetingResponse> _mymeetings;
        private bool _isRunning;
        private DelegateCommand _addmeetingcommand;




        public ShowMeetingsPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.Meetings;
            loadMeeting();
        }

        public DelegateCommand AddMeetingCommand => _addmeetingcommand ?? (_addmeetingcommand = new DelegateCommand(AddMeetingAsync));
        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }
        public ObservableCollection<MeetingItemViewModel> Meetings
        {
            get => _meetings;
            set => SetProperty(ref _meetings, value);
        }
        private async void loadMeeting()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }
            IsRunning = true;
            string url = App.Current.Resources["UrlAPI"].ToString();
            int IdChurch = Int32.Parse(JsonConvert.DeserializeObject<String>(Settings.Church));
            MeetingRequest meetingRequest = new MeetingRequest
            {
                ChurchId = IdChurch
             };
            Response response = await _apiService.GetMeetings<MeetingResponse>(
                url,
                "/api",
                "/Meeting", meetingRequest);
            IsRunning = false;
            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }


            _mymeetings = (List<MeetingResponse>)response.Result;
            Meetings = new ObservableCollection<MeetingItemViewModel>(_mymeetings.Select(p => new MeetingItemViewModel(_navigationService)
            {
                Date=p.Date,
                Church=p.Church,
                Assistances=p.Assistances
            })
         .ToList());
        }

        private async void AddMeetingAsync()
        {
            await _navigationService.NavigateAsync(nameof(AddMeetingPage));
        }
    }
}
