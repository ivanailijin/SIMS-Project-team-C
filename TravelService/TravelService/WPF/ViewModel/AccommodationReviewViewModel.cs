using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.WPF.ViewModel
{
    public class AccommodationReviewViewModel : ViewModelBase
    {
        public ObservableCollection<Guest1> Guests { get; set; }
        public OwnerRatingService _ownerRatingService { get; set; }
        public GuestRatingService _guestRatingService { get; set; }
        public Action CloseAction { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand ShowReviewCommand { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public Owner Owner { get; set; }
        public Guest1 SelectedGuest { get; set; }


        private List<Uri> _ratingImages;

        public List<Uri> RatingImages
        {
            get => _ratingImages;
            set
            {
                if (value != _ratingImages)
                {
                    _ratingImages = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _correctness;
        public string Correctness
        {
            get => _correctness;
            set
            {
                if (value != _correctness)
                {
                    _correctness = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _cleanliness;
        public string Cleanliness
        {
            get => _cleanliness;
            set
            {
                if (value != _cleanliness)
                {
                    _correctness = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _location;
        public string Location
        {
            get => _location;
            set
            {
                if (value != _location)
                {
                    _location = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _comfort;
        public string Comfort
        {
            get => _comfort;
            set
            {
                if (value != _comfort)
                {
                    _comfort = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _content;
        public string Content
        {
            get => _content;
            set
            {
                if (value != _content)
                {
                    _content = value;
                    OnPropertyChanged();
                }
            }
        }

        public AccommodationReviewViewModel(Accommodation selectedAccommodation, Owner owner)
        {
            InitializeCommands();
            SelectedAccommodation = selectedAccommodation;
            this.Owner = owner;
            _ownerRatingService = new OwnerRatingService(Injector.CreateInstance<IOwnerRatingRepository>());
            _guestRatingService = new GuestRatingService(Injector.CreateInstance<IGuestRatingRepository>());

            List<Guest1> guestsList = _ownerRatingService.FindGuestsByAccommodation(SelectedAccommodation);
            List<Guest1> ratedGuests = _guestRatingService.FindRatedGuests(Owner.Id);

            List<Guest1> commonGuests = new List<Guest1>();

            foreach (Guest1 guest in guestsList)
            {
                foreach (Guest1 ratedGuest in ratedGuests)
                {
                    if (guest.Id == ratedGuest.Id)
                    {
                        commonGuests.Add(guest);
                        break;
                    }
                }
            }
            Guests = new ObservableCollection<Guest1>(commonGuests);
        }
        private void InitializeCommands()
        {
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
            ShowReviewCommand = new RelayCommand(Execute_ShowReviewCommand, CanExecute_Command);
        }
        private void Execute_ShowReviewCommand(object obj)
        {
            OwnerRating ownerRating = _ownerRatingService.FindByGuestOwnerIds(SelectedGuest.Id, Owner.Id, SelectedAccommodation.Id);
            Correctness = ownerRating.Correctness.ToString();
            Cleanliness = ownerRating.Cleanliness.ToString();
            Location = ownerRating.Location.ToString();
            Comfort = ownerRating.Comfort.ToString();
            Content = ownerRating.Content.ToString();
            RatingImages = ownerRating.Pictures;
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
