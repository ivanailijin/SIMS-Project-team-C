using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Linq;

using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;

using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class AccommodationReviewViewModel : ViewModelBase
    {
        public ObservableCollection<Guest1> Guests { get; set; }
        public ObservableCollection<OwnerRating> Ratings { get; set; }
        public OwnerRatingService _ownerRatingService { get; set; }
        public GuestRatingService _guestRatingService { get; set; }
        public Guest1Service _guestService { get; set; }
        public AccommodationReview  AccommodationReview { get; set; }
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
        private ObservableCollection<Uri> _displayedImages;

        public ObservableCollection<Uri> DisplayedImages
        {
            get => _displayedImages;
            set
            {
                if (value != _displayedImages)
                {
                    _displayedImages = value;
                    OnPropertyChanged(nameof(DisplayedImages));
                }
            }
        }
        private int currentImageIndex = 0;
        public int CurrentImageIndex
        {
            get { return currentImageIndex; }
            set
            {
                currentImageIndex = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<OwnerRating> _displayedRatings;

        public ObservableCollection<OwnerRating> DisplayedRatings
        {
            get => _displayedRatings;
            set
            {
                if (value != _displayedRatings)
                {
                    _displayedRatings = value;
                    OnPropertyChanged(nameof(DisplayedRatings));
                }
            }
        }
        private int currentCommentIndex = 0;
        public int CurrentCommentIndex
        {
            get { return currentCommentIndex; }
            set
            {
                currentCommentIndex = value;
                OnPropertyChanged();
            }
        }

        private string _averageAccommodationRating;
        public string AverageAccommodationRating
        {
            get => _averageAccommodationRating;
            set
            {
                if (value != _averageAccommodationRating)
                {
                    _averageAccommodationRating = value;
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
                    _cleanliness = value;
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
        private int _numberOfRatings;
        public int NumberOfRatings
        {
            get => _numberOfRatings;
            set
            {
                if (value != _numberOfRatings)
                {
                    _numberOfRatings = value;
                    OnPropertyChanged();
                }
            }
        }
        private RelayCommand _previousImageCommand;
        public RelayCommand PreviousImageCommand
        {
            get => _previousImageCommand;
            set
            {
                if (value != _previousImageCommand)
                {
                    _previousImageCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        private RelayCommand _nextImageCommand;
        public RelayCommand NextImageCommand
        {
            get => _nextImageCommand;
            set
            {
                if (value != _nextImageCommand)
                {
                    _nextImageCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        private RelayCommand _previousCommentCommand;
        public RelayCommand PreviousCommentCommand
        {
            get => _previousCommentCommand;
            set
            {
                if (value != _previousCommentCommand)
                {
                    _previousCommentCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        private RelayCommand _nextCommentCommand;
        public RelayCommand NextCommentCommand
        {
            get => _nextCommentCommand;
            set
            {
                if (value != _nextCommentCommand)
                {
                    _nextCommentCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        public AccommodationReviewViewModel(Accommodation selectedAccommodation, Owner owner, AccommodationReview accommodationReview)
        {
            InitializeCommands();
            SelectedAccommodation = selectedAccommodation;
            this.Owner = owner;
            _ownerRatingService = new OwnerRatingService(Injector.CreateInstance<IOwnerRatingRepository>());
            _guestRatingService = new GuestRatingService(Injector.CreateInstance<IGuestRatingRepository>());
            _guestService = new Guest1Service(Injector.CreateInstance<IGuest1Repository>());
            AccommodationReview = accommodationReview;

            Correctness = Math.Round(_ownerRatingService.GetAverageCorrectness(SelectedAccommodation), 1).ToString();
            Cleanliness = Math.Round(_ownerRatingService.GetAverageCleanliness(SelectedAccommodation), 1).ToString();
            Location = Math.Round(_ownerRatingService.GetAverageLocation(SelectedAccommodation), 1).ToString();
            Comfort = Math.Round(_ownerRatingService.GetAverageComfort(SelectedAccommodation), 1).ToString();
            Content = Math.Round( _ownerRatingService.GetAverageContent(SelectedAccommodation), 1).ToString();
            RatingImages = _ownerRatingService.GetRatingImages(SelectedAccommodation);
            NumberOfRatings = _ownerRatingService.GetNumberOfAccommodationRatings(SelectedAccommodation);
            AverageAccommodationRating = _ownerRatingService.GetAverageAccommodationRating(SelectedAccommodation).ToString();

            List<Guest1> guestsList = _ownerRatingService.FindGuestsByAccommodation(SelectedAccommodation);
            List<Guest1> ratedGuests = _guestRatingService.FindRatedGuests(Owner.Id);

            List<Guest1> commonGuests = _guestService.FindCommonGuests(guestsList, ratedGuests);
            
            Ratings = new ObservableCollection<OwnerRating>(_ownerRatingService.GetFirstTenRatings());
            UpdateDisplayedImages();
            UpdateDisplayedComments();
        }
        private void InitializeCommands()
        {
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
            ShowReviewCommand = new RelayCommand(Execute_ShowReviewCommand, CanExecute_Command);
            PreviousImageCommand = new RelayCommand(Execute_PreviousImageCommand, CanNavigatePrevious);
            NextImageCommand = new RelayCommand(Execute_NextImageCommand, CanNavigateNext);
            NextCommentCommand = new RelayCommand(Execute_NextCommentCommand, CanNavigateNextComment);
            PreviousCommentCommand = new RelayCommand(Execute_PreviousCommentCommand, CanNavigatePreviousComment);
        }
        private void Execute_ShowReviewCommand(object obj)
        {
            OwnerRating ownerRating = _ownerRatingService.FindByGuestOwnerIds(SelectedGuest.Id, Owner.Id, SelectedAccommodation.Id);
        }
        private void Execute_CancelCommand(object obj)
        {
            AccommodationReview.GoBack();
        }
        private void Execute_NextImageCommand(object obj)
        {
            if (CurrentImageIndex < RatingImages.Count - 3)
            {
                CurrentImageIndex++;
                UpdateDisplayedImages();
            }
        }
        private void Execute_PreviousImageCommand(object obj)
        {
            if (CurrentImageIndex > 0)
            {
                CurrentImageIndex--;
                UpdateDisplayedImages();
            }
        }
        private bool CanNavigatePrevious(object arg)
        {
            return CurrentImageIndex > 0;
        }
        private bool CanNavigateNext(object arg)
        {
            return CurrentImageIndex < RatingImages.Count - 1;
        }
        private void Execute_NextCommentCommand(object obj)
        {
            if (CurrentCommentIndex < Ratings.Count - 3)
            {
                CurrentCommentIndex++;
                UpdateDisplayedComments();
            }
        }
        private void Execute_PreviousCommentCommand(object obj)
        {
            if (CurrentCommentIndex > 0)
            {
                CurrentCommentIndex--;
                UpdateDisplayedComments();
            }
        }
        private bool CanNavigatePreviousComment(object arg)
        {
            return CurrentCommentIndex > 0;
        }
        private bool CanNavigateNextComment(object arg)
        {
            return CurrentCommentIndex < Ratings.Count - 3;
        }
        private bool CanExecute_Command(object arg)
        {
            return true;
        }
        private void UpdateDisplayedImages()
        {
            DisplayedImages = new ObservableCollection<Uri>(
                RatingImages.Skip(currentImageIndex).Take(3));
        }
        private void UpdateDisplayedComments()
        {
            DisplayedRatings = new ObservableCollection<OwnerRating>(
                Ratings.Skip(currentCommentIndex).Take(3));
        }
    }
}
