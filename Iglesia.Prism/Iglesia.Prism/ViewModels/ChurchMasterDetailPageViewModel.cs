using Iglesia.Common.Models;
using Iglesia.Prism.Helpers;
using Iglesia.Prism.ItemViewModels;
using Iglesia.Prism.Views;
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

        public ChurchMasterDetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            LoadMenus();
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
