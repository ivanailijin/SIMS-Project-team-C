using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// <summary>
    /// Interaction logic for SendReservationRequestView.xaml
    /// </summary>
    public partial class SendReservationRequestView : Window
    {
        public SendReservationRequestView(ObservableCollection<ReservationRequest> requestsForDelaying, AccommodationReservation selectedReservation, Guest1 guest1)
        {
            InitializeComponent();
            SendReservationRequestViewModel sendReservationRequestView = new SendReservationRequestViewModel(requestsForDelaying, selectedReservation, guest1);
            DataContext = sendReservationRequestView;
            if (sendReservationRequestView.CloseAction == null)
            {
                sendReservationRequestView.CloseAction = new Action(this.Close);
            }
        }
    }
}
