using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using TravelService.Domain.Model;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for AddTour.xaml
    /// </summary>
    public partial class AddTourView : Page, INotifyPropertyChanged
    {
        public AddTourView(Guide guide, bool visibility, NavigationService navigationService)
        {
            InitializeComponent();
           this.DataContext = new AddTourViewModel(this,guide, visibility,navigationService);
            
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
