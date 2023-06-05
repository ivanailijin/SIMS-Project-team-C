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
    public partial class ChooseRequestTypeView : Window, INotifyPropertyChanged
    {
        public ChooseRequestTypeView(Guest2 guest2)
        {
            InitializeComponent();
            ChooseRequestTypeViewModel chooseRequestTypeViewModel = new ChooseRequestTypeViewModel(guest2);
            DataContext = chooseRequestTypeViewModel;
            if (chooseRequestTypeViewModel.CloseAction == null)
            {
                chooseRequestTypeViewModel.CloseAction = new Action(this.Close);
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
