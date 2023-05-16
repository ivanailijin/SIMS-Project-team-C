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

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for AcceptingTourRequestView.xaml
    /// </summary>
    public partial class AcceptingTourRequestView : Window,INotifyPropertyChanged
    {
        public AcceptingTourRequestView(Guide guide,TourRequest selectedTourRequest)
        {
            InitializeComponent();
            AcceptingTourRequestViewModel acceptingTourRequestView = new AcceptingTourRequestViewModel(guide,selectedTourRequest);
            DataContext = acceptingTourRequestView;
            if (acceptingTourRequestView.CloseAction == null)
            {
                acceptingTourRequestView.CloseAction = new Action(this.Close);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
