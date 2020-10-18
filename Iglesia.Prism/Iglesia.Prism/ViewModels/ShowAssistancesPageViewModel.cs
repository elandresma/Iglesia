using Iglesia.Common.Requests;
using Iglesia.Common.Responses;
using Iglesia.Common.Services;
using Iglesia.Prism.Helpers;
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

    public class ShowAssistancesPageViewModel : ViewModelBase
    {
        private MeetingResponse _meeting;
        private List<AssistancesResponse> _assistances;
        private bool _isRunning;
        private readonly IApiService _apiService;
        private DelegateCommand _updateassistancecommand;


        public ShowAssistancesPageViewModel(INavigationService navigationService,IApiService apiService)
                   : base(navigationService)
        {
            _apiService = apiService;
            Title = Languages.Assistances;
        }
        public DelegateCommand UpdateAssistanceCommand => _updateassistancecommand ?? (_updateassistancecommand = new DelegateCommand(UpdateAssistancesasync));

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }
        public MeetingResponse Meeting 
        {
            get => _meeting;
            set => SetProperty(ref _meeting, value);
        }

        public List<AssistancesResponse> Assistances
        {
            get => _assistances;
            set => SetProperty(ref _assistances, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            IsRunning = true;
            if (parameters.ContainsKey("meeting")) 
            {
                Meeting = parameters.GetValue<MeetingResponse>("meeting");
                Title = Meeting.Date.ToString("MMMM d/yyyy");
            }
            LoadAssistances();
        }
        private void LoadAssistances()
        {
            Assistances = Meeting.Assistances.ToList();
            IsRunning = false;
        }
        private async void UpdateAssistancesasync()
        {


               IsRunning = true;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }
            List<AssistancesResponse> AuxiliarAssistences = new List<AssistancesResponse>();

            foreach (var item in Assistances)
            {
              AuxiliarAssistences.Add(item);
            }

            AssistancesRequest request = new AssistancesRequest
            {
                AssistancesList = AuxiliarAssistences,
            };

            string url = App.Current.Resources["UrlAPI"].ToString();

            Response response = await _apiService.UpdateAssistancesAsync<AssistancesResponse>(
                url,
                "/api",
                "/Meeting/UpdateAssistances", request);
            IsRunning = false;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            await App.Current.MainPage.DisplayAlert(Languages.Ok, Languages.UpdateAssistancesMessage, Languages.Accept);

        }
        

    }
}
