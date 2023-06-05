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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelService.Domain.Model;
using TravelService.Repository;
using TravelService.WPF.View;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for PastTours.xaml
    /// </summary>
    public partial class PastTours : Page,INotifyPropertyChanged
    {
        public PastTours(Tour selectedTour,Guide selectedGuide, NavigationService navigationService)
        {
            InitializeComponent();
            this.DataContext = new PastToursViewModel(this, selectedTour, selectedGuide, navigationService);
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
