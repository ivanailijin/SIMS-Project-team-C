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
    /// Interaction logic for AcceptingTourRequestView.xaml
    /// </summary>
    public partial class AcceptingTourRequestView : Page,INotifyPropertyChanged
    {
        public AcceptingTourRequestView(Guide guide,TourRequest selectedTourRequest,NavigationService navigationService)
        {
            InitializeComponent();
            this.DataContext = new AcceptingTourRequestViewModel(guide,selectedTourRequest,this,navigationService);
           
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
