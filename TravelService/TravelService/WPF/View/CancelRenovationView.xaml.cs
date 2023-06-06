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
    /// Interaction logic for CancelRenovationView.xaml
    /// </summary>
    public partial class CancelRenovationView : Window, INotifyPropertyChanged
    {
        public CancelRenovationView(AccommodationRenovation renovation, ScheduledRenovationsCancellationViewModel scheduledRenovationsCancellationViewModel)
        {
            InitializeComponent();
            CancelRenovationViewModel cancelRenovationViewModel = new CancelRenovationViewModel(renovation, scheduledRenovationsCancellationViewModel);
            DataContext = cancelRenovationViewModel;
            if (cancelRenovationViewModel.CloseAction == null)
                cancelRenovationViewModel.CloseAction = new Action(this.Close);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
