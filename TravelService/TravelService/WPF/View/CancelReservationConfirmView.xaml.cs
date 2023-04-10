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
    /// Interaction logic for CancelReservationConfirmView.xaml
    /// </summary>
    public partial class CancelReservationConfirmView : Window
    {
        public CancelReservationConfirmView(ObservableCollection<AccommodationReservation> activeReservations,AccommodationReservation selectedReservation, Guest1 guest1)
        {
            InitializeComponent();
            CancelReservationConfirmViewModel cancelReservationView = new CancelReservationConfirmViewModel(activeReservations,selectedReservation, guest1);
            DataContext = cancelReservationView;
            if (cancelReservationView.CloseAction == null)
            {
                cancelReservationView.CloseAction = new Action(this.Close);
            }
        }
    }
}
