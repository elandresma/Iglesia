using Iglesia.Common.Entities;
using Iglesia.Common.Helpers;
using Iglesia.Common.Requests;
using Iglesia.Common.Responses;
using Iglesia.Common.Services;
using Iglesia.Prism.Helpers;
using Newtonsoft.Json;
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
    public class ModifyUserPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IFilesHelper _filesHelper;
        private ImageSource _image;
        private UserResponse _user;
        private ChurchResponse _church;
        private ObservableCollection<ChurchResponse> _churches;
        private DistrictResponse _district;
        private ObservableCollection<DistrictResponse> _districts;
        private RegionResponse _region;
        private ObservableCollection<RegionResponse> _regions;
        private Profession _profession;
        private ObservableCollection<Profession> _professions;
        private bool _isRunning;
        private bool _isEnabled;
        private MediaFile _file;
        private DelegateCommand _changeImageCommand;
        private DelegateCommand _saveCommand;
        private DelegateCommand _changePasswordCommand;

        public ModifyUserPageViewModel(
            INavigationService navigationService,
            IApiService apiService,
            IFilesHelper filesHelper)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            _filesHelper = filesHelper;
            Title = Languages.ModifyUser;
            IsEnabled = true;
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            User = token.User;
            Image = User.ImageFullPath;
            LoadRegionsAsync();
        }

        public DelegateCommand ChangeImageCommand => _changeImageCommand ??
            (_changeImageCommand = new DelegateCommand(ChangeImageAsync));

        public DelegateCommand SaveCommand => _saveCommand ??
            (_saveCommand = new DelegateCommand(SaveAsync));

        public DelegateCommand ChangePasswordCommand => _changePasswordCommand ??
            (_changePasswordCommand = new DelegateCommand(ChangePasswordAsync));

        public ImageSource Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }
        public Profession Profession
        {
            get => _profession;
            set
            {
                SetProperty(ref _profession, value);
            }
        }
        public ObservableCollection<Profession> Professions
        {
            get => _professions;
            set => SetProperty(ref _professions, value);
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
                        LoadProfessionsAsync();
        }


        private void LoadCurrentRegionDistrictChurch()
        {
            try
            {
                Region = Regions.FirstOrDefault(c => c.Districts.FirstOrDefault(d => d.Churches.FirstOrDefault(ci => ci.Id == User.Church.Id) != null) != null);
                District = Region.Districts.FirstOrDefault(d => d.Churches.FirstOrDefault(c => c.Id == User.Church.Id) != null);
                Church = District.Churches.FirstOrDefault(c => c.Id == User.Church.Id);
                Profession = Professions.FirstOrDefault(p => p.Id == User.Profession.Id);
            }
            catch (Exception ex)
            {

                throw;
            }

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

        private async void SaveAsync()
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

            UserRequest request = new UserRequest
            {
                Address = User.Address,
                ChurchId = Church.Id,
                Document = User.Document,
                Username = User.Email,
                FirstName = User.FirstName,
                ImageArray = imageArray,
                LastName = User.LastName,
                Password = "123456", // Doen't matter, it's only to pass the data annotation
                Phone = User.PhoneNumber,
                ProfessionId = Profession.Id

            };

            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.ModifyUserAsync(url, "/api", "/Account", request, token.Token);
            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                if (response.Message == "Error001")
                {
                    await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.Error001, Languages.Accept);
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

            UserResponse updatedUser = (UserResponse)response.Result;
            token.User = updatedUser;
            Settings.Token = JsonConvert.SerializeObject(token);
           ChurchMasterDetailPageViewModel.GetInstance().LoadUser();
            await App.Current.MainPage.DisplayAlert(Languages.Ok, Languages.ChangeUserMessage, Languages.Accept);
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

            if (string.IsNullOrEmpty(User.PhoneNumber))
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
            if (Profession == null)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ProfessionError, Languages.Accept);
                return false;
            }
            if (Church == null)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ChurchError, Languages.Accept);
                return false;
            }

            return true;
        }
        private async void LoadProfessionsAsync()
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
            Response response = await _apiService.GetListAsync<Profession>(url, "/api", "/Account/GetProfesionsAPI");
            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            List<Profession> list = (List<Profession>)response.Result;
            Professions = new ObservableCollection<Profession>(list.OrderBy(c => c.Name));

                LoadCurrentRegionDistrictChurch();

        }
        private async void ChangePasswordAsync()
        {
        }
    }

}
