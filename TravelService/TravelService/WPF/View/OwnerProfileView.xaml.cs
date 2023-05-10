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
    /// Interaction logic for OwnerProfileView.xaml
    /// </summary>
    public partial class OwnerProfileView : Window
    {
        public OwnerProfileView(Owner owner)
        {
            InitializeComponent();
            OwnerProfileViewModel ownerProfileViewModel = new OwnerProfileViewModel(owner);
            DataContext = ownerProfileViewModel;
            if (ownerProfileViewModel.CloseAction == null)
                ownerProfileViewModel.CloseAction = new Action(this.Close);
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
