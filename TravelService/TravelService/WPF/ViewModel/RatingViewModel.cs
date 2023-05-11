using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class RatingViewModel : ViewModelBase
    {
        private AccommodationReservationService _reservationService;
        public AccommodationReservation SelectedUnratedOwner { get; set; }
        public Guest1 Guest1 { get; set; }
        public Action CloseAction { get; set; }

        private ObservableCollection<AccommodationReservation> _unratedOwners;
        public ObservableCollection<AccommodationReservation> UnratedOwners
        {
            get => _unratedOwners;
            set
            {
                if (value != _unratedOwners)
                {
                    _unratedOwners = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _ownerRatingWindowCommand;
        public RelayCommand OwnerRatingWindowCommand
        {
            get => _ownerRatingWindowCommand;
            set
            {
                if (value != _ownerRatingWindowCommand)
                {
                    _ownerRatingWindowCommand = value;
                    OnPropertyChanged();
                }

            }
        }
        public RatingViewModel(Guest1 guest1)
        {
            this.Guest1 = guest1;
            _reservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());

            List<AccommodationReservation> reservations = new List<AccommodationReservation>(_reservationService.FindUnratedOwners(Guest1.Id));
            reservations = _reservationService.GetLocationData(reservations);
            reservations = _reservationService.GetAccommodationData(reservations);
            reservations = _reservationService.GetOwnerData(reservations);
            UnratedOwners = new ObservableCollection<AccommodationReservation>(reservations);

            OwnerRatingWindowCommand = new RelayCommand(Execute_OwnerRatingWindow, CanExecute_Command);
        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

        private void Execute_OwnerRatingWindow(object sender)
        {
            if (SelectedUnratedOwner != null)
            {
                OwnerRatingView ownerRatingView = new OwnerRatingView(this, SelectedUnratedOwner);
                ownerRatingView.ShowDialog();
            }
            else
            {
                MessageBox.Show("Odaberite smestaj za ocenjivanje!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
