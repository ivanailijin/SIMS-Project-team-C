using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using TravelService.Applications.UseCases;
using TravelService.Domain.Model;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for ApproveReservationRequestView.xaml
    /// </summary>
    public partial class ApproveReservationRequestView : Window, INotifyPropertyChanged
    {
        public ApproveReservationRequestView(ReservationRequest SelectedRequest, ReservationRequestService reservationRequestService, ObservableCollection<ReservationRequest> ReservationRequests)
        {
            InitializeComponent();
            ApproveReservationRequestViewModel approveReservationRequestViewModel = new ApproveReservationRequestViewModel(SelectedRequest, reservationRequestService, ReservationRequests);
            DataContext = approveReservationRequestViewModel;
            if (approveReservationRequestViewModel.CloseAction == null)
                approveReservationRequestViewModel.CloseAction = new Action(this.Close);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
