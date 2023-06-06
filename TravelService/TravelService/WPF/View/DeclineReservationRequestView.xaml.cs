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
using TravelService.Applications.UseCases;
using System.Collections.ObjectModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for DeclineReservationRequestView.xaml
    /// </summary>
    public partial class DeclineReservationRequestView : Window, INotifyPropertyChanged
    {
        public DeclineReservationRequestView(ReservationRequest selectedRequest, ReservationRequestService reservationRequestService, ObservableCollection<ReservationRequest> ReservationRequests )
        {
            InitializeComponent();
            DeclineReservationRequestViewModel declineReservationRequestViewModel = new DeclineReservationRequestViewModel(selectedRequest, reservationRequestService, ReservationRequests);
            DataContext = declineReservationRequestViewModel;
            if (declineReservationRequestViewModel.CloseAction == null)
                declineReservationRequestViewModel.CloseAction = new Action(this.Close);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
