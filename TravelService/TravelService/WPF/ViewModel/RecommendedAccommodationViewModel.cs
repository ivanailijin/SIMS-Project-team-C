using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelService.Application.UseCases;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class RecommendedAccommodationViewModel : ViewModelBase
    {
        public Guest1 Guest1 { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public RecommendedAccommodationView RecommendedAccommodationView { get; set; }
        public int GuestNumber { get; set; }
        public int LengthOfStay { get; set; }

        private List<Accommodation> _accommodations;
        public List<Accommodation> Accommodations
        {
            get => _accommodations;
            set
            {
                if (value != _accommodations)
                {
                    _accommodations = value;
                    OnPropertyChanged();
                    List<Accommodation> accommodations = _accommodations.ToList();
                   // accommodations = _accommodationService.GetOwnerData(accommodations);
                   // accommodations = _accommodationService.SortBySuperowner(accommodations);
                }
            }
        }

        private RelayCommand _accommodationSelectedCommand;
        public RelayCommand AccommodationSelectedCommand
        {
            get => _accommodationSelectedCommand;
            set
            {
                if (value != _accommodationSelectedCommand)
                {
                    _accommodationSelectedCommand = value;
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
        public RecommendedAccommodationViewModel(RecommendedAccommodationView recommendedAccommodationView,List<Accommodation> accommodations, Guest1 guest1, int guestNumber, int lengthOfStay)
        {
            Guest1 = guest1;
            RecommendedAccommodationView = recommendedAccommodationView;
            Accommodations = accommodations;
            GuestNumber = guestNumber;
            LengthOfStay = lengthOfStay;

            AccommodationSelectedCommand = new RelayCommand(Execute_OnItemSelected, CanExecute_Command);
            PreviousPageCommand = new RelayCommand(Execute_PreviousPage, CanExecute_Command);
        }

        public void GoBack()
        {
            RecommendedAccommodationView.GoBack();
        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

        private void Execute_OnItemSelected(object sender)
        {
           // SelectedAccommodationView selectedAccommodationView = new SelectedAccommodationView(SelectedAccommodation, Guest1);
            FirstGuestWindow firstGuestWindow = Window.GetWindow(RecommendedAccommodationView) as FirstGuestWindow ?? new(Guest1);
           // firstGuestWindow?.SwitchToPage(selectedAccommodationView);
        }

        private void Execute_PreviousPage(object sender)
        {
            GoBack();
        }
    }
}
