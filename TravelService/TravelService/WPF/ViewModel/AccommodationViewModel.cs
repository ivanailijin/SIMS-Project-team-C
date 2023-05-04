using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;
using TravelService.WPF.View;
using TravelService.Application.Utils;

namespace TravelService.WPF.ViewModel
{
    public class AccommodationViewModel : ViewModelBase
    {
        private AccommodationService _accommodationService;
        public Guest1 Guest1 { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public ObservableCollection<Accommodation> FilteredAccommodations { get; set; }
        public ObservableCollection<string> Types { get; set; }
        public Action CloseAction { get; set; }

        private ObservableCollection<Accommodation> _accommodations;
        public ObservableCollection<Accommodation> Accommodations
        {
            get => _accommodations;
            set
            {
                if (value != _accommodations)
                {
                    _accommodations = value;
                    OnPropertyChanged();
                    List<Accommodation> accommodations = _accommodations.ToList();
                    accommodations = _accommodationService.GetTypeData(accommodations);
                    accommodations = _accommodationService.GetOwnerData(accommodations);
                    accommodations = _accommodationService.SortBySuperowner(accommodations);
                }
            }
        }

        private RelayCommand _searchWindowCommand;
        public RelayCommand SearchWindowCommand
        {
            get => _searchWindowCommand;
            set
            {
                if (value != _searchWindowCommand)
                {
                    _searchWindowCommand = value;
                    OnPropertyChanged();
                }

            }
        }

        private RelayCommand _reserveCommand;
        public RelayCommand ReserveCommand
        {
            get => _reserveCommand;
            set
            {
                if (value != _reserveCommand)
                {
                    _reserveCommand = value;
                    OnPropertyChanged();
                }

            }
        }

        private RelayCommand _ratingWindowCommand;
        public RelayCommand RatingWindowCommand
        {
            get => _ratingWindowCommand;
            set
            {
                if (value != _ratingWindowCommand)
                {
                    _ratingWindowCommand = value;
                    OnPropertyChanged();
                }

            }
        }

        private RelayCommand _reservationsWindowCommand;
        public RelayCommand ReservationsWindowCommand
        {
            get => _reservationsWindowCommand;
            set
            {
                if (value != _reservationsWindowCommand)
                {
                    _reservationsWindowCommand = value;
                    OnPropertyChanged();
                }

            }
        }

        public AccommodationViewModel(Guest1 guest1)
        {
            this.Guest1 = guest1;

            _accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>());
            List<Accommodation> accommodations = new List<Accommodation>(_accommodationService.GetAll());
            accommodations = _accommodationService.GetLocationData(accommodations);
            accommodations = _accommodationService.GetTypeData(accommodations);
            accommodations = _accommodationService.GetOwnerData(accommodations);
            accommodations = _accommodationService.SortBySuperowner(accommodations);
            Accommodations = new ObservableCollection<Accommodation>(accommodations);

            SearchWindowCommand = new RelayCommand(Execute_SearchWindow, CanExecute_Command);
            ReserveCommand = new RelayCommand(Execute_ReserveWindow, CanExecute_Command);
            RatingWindowCommand = new RelayCommand(Execute_RatingWindow, CanExecute_Command);
            ReservationsWindowCommand = new RelayCommand(Execute_ReservationsWindow, CanExecute_Command);
        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

        private void Execute_SearchWindow(object sender)
        {
            SearchAccommodationView searchAccommodationView = new SearchAccommodationView(this);
            searchAccommodationView.Show();
        }

        private void Execute_ReserveWindow(object sender)
        {
            AccommodationAvailabilityView accommodationAvailabilityView = new AccommodationAvailabilityView(SelectedAccommodation, Guest1);
            accommodationAvailabilityView.Show();
        }

        private void Execute_RatingWindow(object sender)
        {
            RatingView ratingView = new RatingView(Guest1);
            ratingView.Show();
        }

        private void Execute_ReservationsWindow(object sender)
        {
            ReservationsView reservationsView = new ReservationsView(Guest1);
            reservationsView.Show();
        }
    }
}
