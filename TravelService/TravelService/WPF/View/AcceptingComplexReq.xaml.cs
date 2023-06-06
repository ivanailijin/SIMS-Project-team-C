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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelService.Domain.Model;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for AcceptingComplexReq.xaml
    /// </summary>
    public partial class AcceptingComplexReq : Page, INotifyPropertyChanged
    {
        public AcceptingComplexReq(TourRequest selected,Guide guide, ComplexTourRequest selectedComplex, NavigationService navigationService)
        {
            InitializeComponent();
            this.DataContext = new AcceptingComplexREqViewModel(selected,guide, selectedComplex, this, navigationService);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
