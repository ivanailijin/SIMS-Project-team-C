using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.WPF.ViewModel
{
    public class AddLocationViewModel : ViewModelBase
    {
        public int LocationId;
        private readonly LocationService _locationService;
        private readonly TourService _tourService;
        public List<Location> Locations { get; set; } 
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public Action CloseAction { get; set; }

        private Location _selectedLocation; 

        public Location SelectedLocation
        {
            get => _selectedLocation;
            set
            {
                if (value != _selectedLocation)
                {
                    _selectedLocation = value;
                    OnPropertyChanged();
                }
            }
        }
        public AddLocationViewModel(int Id)
        {
            _tourService = new TourService(Injector.CreateInstance<ITourRepository>());
            _locationService= new LocationService(Injector.CreateInstance<ILocationRepository>());
            LocationId = Id;

            Locations = _locationService.GetAll();
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
            SaveCommand = new RelayCommand(Execute_CreateCommand, CanExecute_Command);
        }


        private void Execute_CreateCommand(object obj)
        {

            if (SelectedLocation == null)
            {
                MessageBox.Show("Please select language first.");
                return;
            }
            Location selectedLocation = SelectedLocation;
            selectedLocation.Id = LocationId;
            Location savedLocation = _locationService.Save(selectedLocation);

        }
        private void Execute_CancelCommand(object obj)
        {
            CloseAction();
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }
    }
}