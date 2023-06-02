using System;
using System.Windows;
using System.Windows.Controls;
using TravelService.Domain.Model;
using TravelService.WPF.Services;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for SelectedAccommodationView.xaml
    /// </summary>
    public partial class SelectedAccommodationView : Page, INavigationInterface
    {
        public SelectedAccommodationView(Accommodation selectedAccommodation, Guest1 guest1)
        {
            InitializeComponent();
            SelectedAccommodationViewModel selectedAccommodationViewModel = new SelectedAccommodationViewModel(this, selectedAccommodation, guest1);
            DataContext = selectedAccommodationViewModel;
        }

        public void GoBack()
        {
            NavigationService?.GoBack();
        }
    }
}
