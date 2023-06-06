using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class ClosingAccommodationViewModel : ViewModelBase
    {
        public AccommodationService _accommodationService { get; set;}
        public ObservableCollection<Accommodation> Accommodations { get; set;}
        public Accommodation SelectedAccommodation { get; set;}
        public Location Location { get; set;}
        public ClosingAccommodationView ClosingAccommodationView { get; set;}
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand CloseAccommodationCommand { get; set; }

        public ClosingAccommodationViewModel(Location location, ClosingAccommodationView closingAccommodationView)
        {
            InitializeCommands();
            _accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>());
            Location = location;
            ClosingAccommodationView = closingAccommodationView;
            Accommodations = new ObservableCollection<Accommodation>(_accommodationService.GetAccommodationsByLocation(Location));
        }

        private void InitializeCommands()
        {
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
            CloseAccommodationCommand = new RelayCommand(Execute_CloseAccommodationCommand, CanExecute_Command);
        }
        private void Execute_CloseAccommodationCommand(object obj)
        {
            MessageBoxResult result = MessageBox.Show($"Da li ste sigurni da zelite da uklonite smestaj {SelectedAccommodation.Name}?", "Uklanjanje smestaja", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes)
            {
                _accommodationService.Delete(SelectedAccommodation);
                Accommodations.Remove(SelectedAccommodation);
            }
        }
        private void Execute_CancelCommand(object obj)
        {
            ClosingAccommodationView.GoBack();
        }
        private bool CanExecute_Command(object arg)
        {
            return true;
        }
    }
}
