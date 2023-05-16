using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TravelService.Domain.Model;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    public partial class SecondGuestNotificationsView : Window, INotifyPropertyChanged
    {
        public SecondGuestNotificationsView(NewTourNotification selectedNotification, Guest2 guest2)
        {
            InitializeComponent();
            SecondGuestNotificationsViewModel secondGuestNotificationsViewModel = new SecondGuestNotificationsViewModel(selectedNotification, guest2);
            DataContext = secondGuestNotificationsViewModel;
            if (secondGuestNotificationsViewModel.CloseAction == null)
            {
                secondGuestNotificationsViewModel.CloseAction = new Action(this.Close);
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
