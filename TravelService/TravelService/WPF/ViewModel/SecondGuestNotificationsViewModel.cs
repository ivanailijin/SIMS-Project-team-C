using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.WPF.ViewModel
{
    public class SecondGuestNotificationsViewModel : ViewModelBase
    {
        private readonly NewTourNotificationService _notificationService;
        public NewTourNotification SelectedNotification { get; set; }
        public Guest2 Guest2 { get; set; }
        public Action CloseAction { get; set; }
        public ObservableCollection<NewTourNotification> Notifications { get; set; }

        private RelayCommand _trackCommand;
        public RelayCommand TrackCommand
        {
            get => _trackCommand;
            set
            {
                if (value != _trackCommand)
                {
                    _trackCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        public SecondGuestNotificationsViewModel(NewTourNotification selectedNotification, Guest2 guest2) 
        {
            _notificationService = new NewTourNotificationService(Injector.CreateInstance<INewTourNotificationRepository>());
            List<NewTourNotification> notifications = new List<NewTourNotification>(_notificationService.GetAll());
            Notifications = new ObservableCollection<NewTourNotification>(_notificationService.GetGuestsNotifications(notifications, guest2));
            Guest2 = guest2;
            SelectedNotification = selectedNotification;
        }
    }
}
