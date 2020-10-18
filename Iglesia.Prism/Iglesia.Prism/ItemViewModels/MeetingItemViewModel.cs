using Iglesia.Common.Helpers;
using Iglesia.Common.Responses;
using Iglesia.Prism.ViewModels;
using Iglesia.Prism.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Iglesia.Prism.ItemViewModels
{
    public class MeetingItemViewModel : MeetingResponse
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _DetailMeetingCommand;



        public MeetingItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand DetailMeetingCommand => _DetailMeetingCommand ?? (_DetailMeetingCommand = new DelegateCommand(DetailMeetingAsync));

        private async void DetailMeetingAsync()
        {
            NavigationParameters parameters = new NavigationParameters
                {
                    { "meeting", this }
                };

            await _navigationService.NavigateAsync(nameof(ShowAssistancesPage),parameters);
        }
    }

}

