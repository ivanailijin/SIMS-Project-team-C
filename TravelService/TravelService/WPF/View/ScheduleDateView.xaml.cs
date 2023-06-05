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
    /// Interaction logic for ScheduleDate.xaml
    /// </summary>
    public partial class ScheduleDateView : Page, INotifyPropertyChanged
    {
        public ScheduleDateView(TourRequest selectedTourRequest,NavigationService navigationService )
        {
            InitializeComponent();
          this.DataContext= new ScheduleDateViewModel(selectedTourRequest,navigationService,this);
          
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
