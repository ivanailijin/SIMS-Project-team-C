using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class SecondGuestNotificationsViewModel : ViewModelBase
    {
        private readonly NewTourNotificationService _notificationService;
        public NewTourNotification SelectedNotification { get; set; }
        public Guest2 Guest2 { get; set; }
        public Action CloseAction { get; set; }
        public ObservableCollection<NewTourNotification> Notifications { get; set; }

        private RelayCommand _checkNotificationsCommand;
        public RelayCommand CheckNotificationsCommand
        {
            get => _checkNotificationsCommand;
            set
            {
                if (value != _checkNotificationsCommand)
                {
                    _checkNotificationsCommand = value;
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
            CheckNotificationsCommand = new RelayCommand(Execute_CheckNotificationsCommand, CanExecute_Command);
        }
        private bool CanExecute_Command(object parameter)
        {
            return true;
        }
        private void Execute_CheckNotificationsCommand(object sender)
        {
            ShowGuestsNotificationView showGuestsNotificationView = new ShowGuestsNotificationView(SelectedNotification, Guest2);
            showGuestsNotificationView.Show();
            CloseAction();
        }
    }
}
