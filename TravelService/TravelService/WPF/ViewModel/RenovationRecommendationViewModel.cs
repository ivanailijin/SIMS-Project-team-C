using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Application.UseCases;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Application.Utils;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.WPF.ViewModel
{
    public class RenovationRecommendationViewModel : ViewModelBase
    {
        private RenovationRecommendationService _renovationRecommendationService;
        private OwnerService _ownerService;
        public AccommodationReservation SelectedUnratedOwner { get; set; }
        public Action CloseAction { get; set; }

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

        public RenovationRecommendationViewModel(AccommodationReservation selectedUnratedOwner)
        {
            SelectedUnratedOwner = selectedUnratedOwner;
            _renovationRecommendationService = new RenovationRecommendationService(Injector.CreateInstance<IRenovationRecommendationRepository>());
            _ownerService = new OwnerService(Injector.CreateInstance<IOwnerRepository>());

            AccommodationName = selectedUnratedOwner.Accommodation.Name;
            Owner owner = _ownerService.FindById(selectedUnratedOwner.OwnerId);
            OwnerName = owner.Username;

            PreviousPageCommand = new RelayCommand(Execute_PreviousPage, CanExecute_Command);
        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

        private void Execute_PreviousPage(object sender)
        {
            CloseAction();
        }
    }
}
