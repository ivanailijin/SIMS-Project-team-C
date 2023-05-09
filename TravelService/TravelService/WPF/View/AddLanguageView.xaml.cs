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
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for AddLocationView.xaml
    /// </summary>
    public partial class AddLanguageView : Window, INotifyPropertyChanged
    {
        public AddLanguageView(int Id)
        {
            InitializeComponent();
            AddLanguageViewModel addLanguageViewModel = new AddLanguageViewModel(Id);
            DataContext = addLanguageViewModel;
            if (addLanguageViewModel.CloseAction == null)
            {
                addLanguageViewModel.CloseAction = new Action(this.Close);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
