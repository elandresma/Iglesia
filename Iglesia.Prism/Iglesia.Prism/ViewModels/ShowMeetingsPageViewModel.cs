using Iglesia.Prism.Helpers;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Iglesia.Prism.ViewModels
{
    public class ShowMeetingsPageViewModel :ViewModelBase
    {
        public ShowMeetingsPageViewModel(INavigationService navigationService)
                   : base(navigationService)
        {
            Title = Languages.Meetings;
        }
    }
}
