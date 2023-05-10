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
    /// Interaction logic for UpdateOwnerProfileView.xaml
    /// </summary>
    public partial class UpdateOwnerProfileView : Window
    {
        public UpdateOwnerProfileView(Owner owner)
        {
            InitializeComponent();
            UpdateOwnerProfileViewModel updateOwnerProfileViewModel = new UpdateOwnerProfileViewModel(owner);
            DataContext = updateOwnerProfileViewModel;
            if (updateOwnerProfileViewModel.CloseAction == null)
                updateOwnerProfileViewModel.CloseAction = new Action(this.Close);
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
