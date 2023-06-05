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
using TravelService.Domain.Model;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    public partial class AddComplexTourRequestView : Window, INotifyPropertyChanged
    {
        public AddComplexTourRequestView(Guest2 guest2, ObservableCollection<TourRequest> tourRequests)
        {
            InitializeComponent();
            AddComplexTourRequestViewModel addComplexTourRequestViewModel = new AddComplexTourRequestViewModel(guest2, tourRequests);
            DataContext = addComplexTourRequestViewModel;
            if (addComplexTourRequestViewModel.CloseAction == null)
            {
                addComplexTourRequestViewModel.CloseAction = new Action(this.Close);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
