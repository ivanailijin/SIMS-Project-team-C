using System;
using System.Windows;
using TravelService.Domain.Model;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for SelectedAccommodationView.xaml
    /// </summary>
    public partial class SelectedAccommodationView : Window
    {
        public SelectedAccommodationView(Accommodation selectedAccommodation, Guest1 guest1)
        {
            InitializeComponent();
            SelectedAccommodationViewModel selectedAccommodationViewModel = new SelectedAccommodationViewModel(selectedAccommodation, guest1);
            DataContext = selectedAccommodationViewModel;
            if (selectedAccommodationViewModel.CloseAction == null)
            {
                selectedAccommodationViewModel.CloseAction = new Action(this.Close);
            }
        }

    }
}
