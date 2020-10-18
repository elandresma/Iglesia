using Iglesia.Common.Helpers;
using Iglesia.Common.Requests;
using Iglesia.Common.Responses;
using Iglesia.Common.Services;
using Iglesia.Prism.Helpers;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;

namespace Iglesia.Prism.ViewModels
{
    public class UsersPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private ObservableCollection<UserResponse> _user;
        private bool _isRunning;
        private UserResponse _currentUser;
        private string _search;
        private List<UserResponse> _myusers;
        private DelegateCommand _searchCommand;
        public DelegateCommand SearchCommand => _searchCommand ?? (_searchCommand = new DelegateCommand(ShowUsers));


        public UsersPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.Users;
            LoadUser();
            GetUsersByChurch();
        }


        public string Search
        {
            get => _search;
            set
            {
                SetProperty(ref _search, value);
                ShowUsers();
            }
        }
        public UserResponse User
        {
            get => _currentUser;
            set => SetProperty(ref _currentUser, value);
        }

        private void LoadUser()
        {
            if (Settings.IsLogin)
            {
                TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
                User = token.User;
            }
        }

        private void ShowUsers()
        {
            if (string.IsNullOrEmpty(Search))
            {
                Users = new ObservableCollection<UserResponse>(_myusers);
            }
            else
            {
                Users = new ObservableCollection<UserResponse>(_myusers
                    .Where(p => p.FullName.ToLower().Contains(Search.ToLower())));
            }
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
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
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }
            IsRunning = true;
            string url = App.Current.Resources["UrlAPI"].ToString();
            EmailRequest teacherEmail = new EmailRequest
            {
                Email = User.Email
            };
            Response response = await _apiService.GetUsersAsync<UserResponse>(
                url,
                "/api",
                "/Churches/GetUsersByChurch", teacherEmail);
            IsRunning = false;
            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }


            _myusers = (List<UserResponse>)response.Result;
            ShowUsers();
            Settings.Church = JsonConvert.SerializeObject(_myusers.FirstOrDefault().Church.Id);
        }
    }

}
