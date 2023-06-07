using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class NotificationViewModel : ViewModelBase
    {
        public Owner Owner { get; set; }
        public NotificationView NotificationView { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand ShowDetailsCommand { get; set; }
        public Notification SelectedNotification { get; set; }
        public ObservableCollection<Notification> Notifications { get; set; }
        public NotificationViewModel(Owner owner, NotificationView notificationView)
        {
            InitializeCommands();
            this.Owner = owner;
            this.NotificationView = notificationView;
            Notification notification1 = new Notification("Jovan Jovanovic", new Location("Srbija", "Novi Sad"), DateOnly.Parse("23-May-2023"), false, "Hotel Novi Sad");
            Notification notification2 = new Notification("Petar Petrovic", new Location("Srbija", "Leskovac"), DateOnly.Parse("13-Feb-2023"), true, "");
            Notification notification3 = new Notification("Milica Milic", new Location("Italija", "Verona"), DateOnly.Parse("18-Sep-2022"), true, "");
            Notification notification4 = new Notification("Ivana Ilijin Jovanovic", new Location("Srbija", "Zlatibor"), DateOnly.Parse("23-May-2023"), false, "Koliba Zlatibor");
            Notifications = new ObservableCollection<Notification>();
            Notifications.Add(notification1);
            Notifications.Add(notification2);
            Notifications.Add(notification3);
            Notifications.Add(notification4);

            foreach(Notification notif in Notifications)
            {
                if (notif.ForumNotification)
                {
                    notif.Content = "Gost " + notif.UserName + " je otvorio novi forum na lokaciji " + notif.Location.CityAndCountry;
                }
                else
                {
                    notif.Content = "Gost " + notif.UserName + " je " + notif.Date.ToString() + " napustio smestaj " + notif.AccommodationName;
                }
            }

        }
        private void InitializeCommands()
        {
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
            ShowDetailsCommand = new RelayCommand(Execute_ShowDetailsCommand, CanExecute_Command);
        }
        private void Execute_ShowDetailsCommand(object obj)
        {
            if (!SelectedNotification.ForumNotification)
            {
                GuestRatingOverview ratingOverview = new GuestRatingOverview(Owner);
                OwnerWindow ownerWindow = Window.GetWindow(NotificationView) as OwnerWindow ?? new(Owner);
                ownerWindow?.SwitchToPage(ratingOverview);
            }
            else
            {
                ForumSelectionView forumSelectionView = new ForumSelectionView(Owner);
                OwnerWindow ownerWindow = Window.GetWindow(NotificationView) as OwnerWindow ?? new(Owner);
                ownerWindow?.SwitchToPage(forumSelectionView);
            }
        }
        private void Execute_CancelCommand(object obj)
        {
            NotificationView.GoBack();
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }
    }
}
