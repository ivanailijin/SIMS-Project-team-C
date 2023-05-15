using System;
using System.Windows;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.WPF.ViewModel
{
    public class RenovationRecommendationViewModel : ViewModelBase
    {
        private RenovationRecommendationService _renovationRecommendationService;
        private OwnerService _ownerService;
        private OwnerRatingService _ownerRatingService;
        public OwnerRating Rating { get; set; }
        public AccommodationReservation SelectedUnratedOwner { get; set; }
        public Action CloseAction { get; set; }
        public Action CloseParentWindow { get; set; }

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

        private string _ownerName;
        public string OwnerName
        {
            get => _ownerName;
            set
            {
                if (value != _ownerName)
                {
                    _ownerName = value;
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

        private int _urgencyLevel;
        public int UrgencyLevel
        {
            get => _urgencyLevel;
            set
            {
                if (value != _urgencyLevel)
                {
                    _urgencyLevel = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _previousPageCommand;
        public RelayCommand PreviousPageCommand
        {
            get => _previousPageCommand;
            set
            {
                if (value != _previousPageCommand)
                {
                    _previousPageCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _sendRecommendationCommand;
        public RelayCommand SendRecommendationCommand
        {
            get => _sendRecommendationCommand;
            set
            {
                if (value != _sendRecommendationCommand)
                {
                    _sendRecommendationCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        public RenovationRecommendationViewModel(Action closeParentWindow, AccommodationReservation selectedUnratedOwner, OwnerRating rating)
        {
            SelectedUnratedOwner = selectedUnratedOwner;
            CloseParentWindow = closeParentWindow;
            _renovationRecommendationService = new RenovationRecommendationService(Injector.CreateInstance<IRenovationRecommendationRepository>());
            _ownerService = new OwnerService(Injector.CreateInstance<IOwnerRepository>());
            _ownerRatingService = new OwnerRatingService(Injector.CreateInstance<IOwnerRatingRepository>());
            Rating = rating;

            AccommodationName = selectedUnratedOwner.Accommodation.Name;
            Owner owner = _ownerService.FindById(selectedUnratedOwner.OwnerId);
            OwnerName = owner.Username;

            PreviousPageCommand = new RelayCommand(Execute_PreviousPage, CanExecute_Command);
            SendRecommendationCommand = new RelayCommand(Execute_SendRecommendation, CanExecute_Command);
        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

        private void Execute_PreviousPage(object sender)
        {
            CloseAction();
        }

        private void Execute_SendRecommendation(object sender)
        {
            if (string.IsNullOrWhiteSpace(Comment) ||
                string.IsNullOrWhiteSpace(UrgencyLevel.ToString()))
            {
                MessageBox.Show("Niste popunili sve parametre za preporuku", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                RenovationRecommendation renovationRecommendation = new RenovationRecommendation(SelectedUnratedOwner.AccommodationId, Comment, UrgencyLevel);
                _renovationRecommendationService.Save(renovationRecommendation);
                Rating.RenovationRecommendationId = renovationRecommendation.Id;
                _ownerRatingService.Save(Rating);
                CloseAction();
                CloseParentWindow();
            }
        }

    }
}
