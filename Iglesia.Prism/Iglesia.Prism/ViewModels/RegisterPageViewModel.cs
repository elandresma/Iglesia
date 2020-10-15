using Iglesia.Common.Helpers;
using Iglesia.Common.Requests;
using Iglesia.Common.Responses;
using Iglesia.Common.Services;
using Iglesia.Prism.Helpers;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Iglesia.Prism.ViewModels
{
    public class RegisterPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IRegexHelper _regexHelper;
        private readonly IApiService _apiService;
        private ImageSource _image;
        private UserRequest _user;
        private ChurchResponse _church;
        private ObservableCollection<ChurchResponse> _churches;
        private DistrictResponse _district;
        private ObservableCollection<DistrictResponse> _districts;
        private RegionResponse _region;
        private ObservableCollection<RegionResponse> _regions;
        private bool _isRunning;
        private bool _isEnabled;
        private DelegateCommand _registerCommand;
        private readonly IFilesHelper _filesHelper;
        private MediaFile _file;
        private DelegateCommand _changeImageCommand;


        public RegisterPageViewModel(
            INavigationService navigationService,
            IRegexHelper regexHelper,
            IApiService apiService, 
            IFilesHelper filesHelper)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _regexHelper = regexHelper;
            _apiService = apiService;
            _filesHelper = filesHelper;
            Title = Languages.Register;
            Image = App.Current.Resources["UrlNoImage"].ToString();
            IsEnabled = true;
            User = new UserRequest();
            LoadRegionsAsync();

        }
        public DelegateCommand ChangeImageCommand => _changeImageCommand ?? (_changeImageCommand = new DelegateCommand(ChangeImageAsync));

        public DelegateCommand RegisterCommand => _registerCommand ?? (_registerCommand = new DelegateCommand(RegisterAsync));

        public ImageSource Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public UserRequest User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }


        public RegionResponse Region
        {
            get => _region;
            set
            {
                Districts = value != null ? new ObservableCollection<DistrictResponse>(value.Districts) : null;
                Churches = new ObservableCollection<ChurchResponse>();
                District = null;
                Church = null;
                SetProperty(ref _region, value);
            }
        }


        public ObservableCollection<RegionResponse> Regions
        {
            get => _regions;
            set => SetProperty(ref _regions, value);
        }

        public DistrictResponse District
        {
            get => _district;
            set
            {
                Churches = value != null ? new ObservableCollection<ChurchResponse>(value.Churches) : null;
                Church = null;
                SetProperty(ref _district, value);
            }
        }

        public ObservableCollection<DistrictResponse> Districts
        {
            get => _districts;
            set => SetProperty(ref _districts, value);
        }

        public ChurchResponse Church
        {
            get => _church;
            set => SetProperty(ref _church, value);
        }

        public ObservableCollection<ChurchResponse> Churches
        {
            get => _churches;
            set => SetProperty(ref _churches, value);
        }
        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        private async void LoadRegionsAsync()
        {
            IsRunning = true;
            IsEnabled = false;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }

            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.GetListAsync<RegionResponse>(url, "/api", "/Regions");
            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            List<RegionResponse> list = (List<RegionResponse>)response.Result;
            Regions = new ObservableCollection<RegionResponse>(list.OrderBy(c => c.Name));
        }

        private async void RegisterAsync()
        {
            bool isValid = await ValidateDataAsync();
            if (!isValid)
            {
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }

            byte[] imageArray = null;
            if (_file != null)
            {
                imageArray = _filesHelper.ReadFully(_file.GetStream());
            }

            User.ImageArray = imageArray;

            string url = App.Current.Resources["UrlAPI"].ToString();
            User.ChurchId = Church.Id;

            Response response = await _apiService.RegisterUserAsync(url, "/api", "/Account/Register", User);
            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                if (response.Message == "Error003")
                {
                    await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.Error003, Languages.Accept);
                }
                else if (response.Message == "Error004")
                {
                    await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.Error004, Languages.Accept);
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                }

                return;
            }

            await App.Current.MainPage.DisplayAlert(Languages.Ok, Languages.RegisterMessage, Languages.Accept);
            await _navigationService.GoBackAsync();

        }

        private async Task<bool> ValidateDataAsync()
        {
            if (string.IsNullOrEmpty(User.Document))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.DocumentError, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(User.FirstName))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.FirstNameError, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(User.LastName))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.LastNameError, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(User.Address))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.AddressError, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(User.Username) || !_regexHelper.IsValidEmail(User.Username))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.EmailError, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(User.Phone))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.PhoneError, Languages.Accept);
                return false;
            }

            if (Region == null)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.RegionError, Languages.Accept);
                return false;
            }

            if (District == null)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.DistrictError, Languages.Accept);
                return false;
            }

            if (Church == null)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ChurchError, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(User.Password) || User.Password?.Length < 6)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.PasswordError, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(User.PasswordConfirm))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.PasswordConfirmError1, Languages.Accept);
                return false;
            }

            if (User.Password != User.PasswordConfirm)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.PasswordConfirmError2, Languages.Accept);
                return false;
            }

            return true;
        }
        private async void ChangeImageAsync()
        {
            await CrossMedia.Current.Initialize();

            string source = await Application.Current.MainPage.DisplayActionSheet(
                Languages.PictureSource,
                Languages.Cancel,
                null,
                Languages.FromGallery,
                Languages.FromCamera);

            if (source == Languages.Cancel)
            {
                _file = null;
                return;
            }

            if (source == Languages.FromCamera)
            {
                if (!CrossMedia.Current.IsCameraAvailable)
                {
                    await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.NoCameraSupported, Languages.Accept);
                    return;
                }

                _file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,
                    }
                );
            }
            else
            {
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.NoGallerySupported, Languages.Accept);
                    return;
                }

                _file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (_file != null)
            {
                Image = ImageSource.FromStream(() =>
                {
                    System.IO.Stream stream = _file.GetStream();
                    return stream;
                });
            }
        }

    }


}
