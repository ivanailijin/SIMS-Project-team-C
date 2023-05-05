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
using TravelService.Application.Utils;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.ViewModel;

namespace TravelService.View
{
    /// <summary>
    /// Interaction logic for MovingReservationRequestsView.xaml
    /// </summary>
    public partial class MovingReservationRequestsView : Window, INotifyPropertyChanged
    {
        
        public MovingReservationRequestsView()
        {
            InitializeComponent();
            MovingReservationRequestsViewModel movingReservationRequestsViewModel = new MovingReservationRequestsViewModel();
            DataContext = movingReservationRequestsViewModel;
            if (movingReservationRequestsViewModel.CloseAction == null)
                movingReservationRequestsViewModel.CloseAction = new Action(this.Close);

        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
