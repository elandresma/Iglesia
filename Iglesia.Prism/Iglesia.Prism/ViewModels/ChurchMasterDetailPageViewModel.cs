using Iglesia.Common.Helpers;
using Iglesia.Common.Models;
using Iglesia.Common.Responses;
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

namespace Iglesia.Prism.ViewModels
{
    public class ChurchMasterDetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private UserResponse _user;
        private static ChurchMasterDetailPageViewModel _instance;


        public ChurchMasterDetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _instance = this;
            _navigationService = navigationService;
            LoadMenus();
            LoadUser();
        }

        public static ChurchMasterDetailPageViewModel GetInstance()
        {
            return _instance;
        }

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }
        public void LoadUser()
        {
            if (Settings.IsLogin)
            {
                TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
                User = token.User;
            }
        }
        public ObservableCollection<MenuItemViewModel> Menus { get; set; }

        private void LoadMenus()
        {
            List<Menu> menus = new List<Menu>
        {
            new Menu
            {
                Icon = "ic_Meeting",
                PageName = $"{nameof(ShowMeetingsPage)}",
                Title = Languages.Meetings,
                IsLoginRequired = true
            },
            new Menu
            {
                Icon = "ic_UsersByChurch",
                PageName = $"{nameof(UsersPage)}",
                Title = Languages.ShowUsersByChurch,
                 IsLoginRequired = true
            },
            new Menu
            {
                Icon = "ic_User",
                PageName = $"{nameof(ModifyUserPage)}",
                Title = Languages.ModifyUser,
                IsLoginRequired = true
            },
            new Menu
            {
                Icon = "ic_exit",
                PageName = $"{nameof(LoginPage)}",
                Title = Languages.Logout
            }
        };

            Menus = new ObservableCollection<MenuItemViewModel>(
                menus.Select(m => new MenuItemViewModel(_navigationService)
                {
                    Icon = m.Icon,
                    PageName = m.PageName,
                    Title = m.Title,
                    IsLoginRequired = m.IsLoginRequired
                }).ToList());
        }
    }

}
