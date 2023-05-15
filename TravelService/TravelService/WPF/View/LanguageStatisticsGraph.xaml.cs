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
    public partial class LanguageStatisticsGraph : Window, INotifyPropertyChanged
    {
        public LanguageStatisticsGraph(Guest2 guest2)
        {
            InitializeComponent();
            LanguageStatisticsGraphViewModel languageStatisticsGraphViewModel = new LanguageStatisticsGraphViewModel(guest2);
            DataContext = languageStatisticsGraphViewModel;
            if (languageStatisticsGraphViewModel.CloseAction == null)
            {
                languageStatisticsGraphViewModel.CloseAction = new Action(this.Close);
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
