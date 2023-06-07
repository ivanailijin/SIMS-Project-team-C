using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class GuestRatingViewModel
    {
        private readonly GuestRatingService _guestRatingService;

        private readonly AccommodationReservationService _reservationService;

        private readonly AccommodationService _accommodationService;

        private readonly Guest1Service _guest1Service;

        public ObservableCollection<AccommodationReservation> UnratedReservations { get; set; } 
        public GuestRatingView GuestRatingView { get; set; }
        public Action CloseAction { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand AddGuestRatingCommand { get; set; }
        public Owner Owner { get; set; }
        public Guest1 Guest { get; set; }

        public Window Parent { get; set; }

        public AccommodationReservation SelectedReservation { get; set; }

        private int ReservationId;

        private int _cleanness;
        public int Cleanness
        {
            get => _cleanness;
            set
            {
                if (value != _cleanness)
                {
                    _cleanness = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _rulesFollowing;
        public int RulesFollowing
        {
            get => _rulesFollowing;
            set
            {
                if (value != _rulesFollowing)
                {
                    _rulesFollowing = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _noiseLevel;
        public int NoiseLevel
        {
            get => _noiseLevel;
            set
            {
                if (value != _noiseLevel)
                {
                    _noiseLevel = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _communication;
        public int Communication
        {
            get => _communication;
            set
            {
                if (value != _communication)
                {
                    _communication = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _propertyRespect;
        public int PropertyRespect
        {
            get => _propertyRespect;
            set
            {
                if (value != _propertyRespect)
                {
                    _propertyRespect = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _comment;
        public string Comment
        {
            get => _comment;
            set
            {
                if (value != _comment)
                {
                    _comment = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _accommodationName;
        public string AccommodationName
        {
            get => _accommodationName;
            set
            {
                if (value != _accommodationName)
                {
                    _accommodationName = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _guestName;
        public string GuestName
        {
            get => _guestName;
            set
            {
                if (value != _guestName)
                {
                    _guestName = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime _checkOutDate;
        public DateTime CheckOutDate
        {
            get => _checkOutDate;
            set
            {
                if (value != _checkOutDate)
                {
                    _checkOutDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _checkInDate;
        public DateTime CheckInDate
        {
            get => _checkInDate;
            set
            {
                if (value != _checkInDate)
                {
                    _checkInDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public GuestRatingViewModel(AccommodationReservation selectedReservation, Owner owner, GuestRatingView guestRatingView, ObservableCollection<AccommodationReservation> unratedReservations) 
        {
            InitializeCommands();
            SelectedReservation = selectedReservation;
            ReservationId = selectedReservation.Id;
            this.Owner = owner;
            GuestRatingView = guestRatingView;
            UnratedReservations = unratedReservations;
            _guestRatingService = new GuestRatingService(Injector.CreateInstance<IGuestRatingRepository>());
            _reservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
            _accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>());
            _guest1Service = new Guest1Service(Injector.CreateInstance<IGuest1Repository>());

            Guest = _guest1Service.FindById(SelectedReservation.GuestId);
            AccommodationReservation ratedReservation = _reservationService.FindById(ReservationId);
            Accommodation accommodation = _accommodationService.FindById(ratedReservation.AccommodationId);

            AccommodationName = accommodation.Name;
            CheckInDate = ratedReservation.CheckInDate;
            CheckOutDate = ratedReservation.CheckOutDate;
            GuestName = Guest.Username;
        }

        private void InitializeCommands()
        {
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
            AddGuestRatingCommand = new RelayCommand(Execute_AddGuestRatingCommand, CanExecute_Command);
        }

        private void Execute_AddGuestRatingCommand(object obj)
        {
            GuestRating guestRating = new GuestRating(Owner.Id, Guest.Id, Cleanness, RulesFollowing, Communication, NoiseLevel, PropertyRespect, Comment, ReservationId);
            _guestRatingService.Save(guestRating);
            AccommodationReservation ratedReservation = _reservationService.FindById(ReservationId);
            ratedReservation.IsRated = true;
            _reservationService.Update(ratedReservation);
            UnratedReservations.Remove(ratedReservation);

            GuestRatingView.GoBack();
        }

        private void Execute_CancelCommand(object obj)
        {
            GuestRatingView.GoBack();
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }
    }
}
