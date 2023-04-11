using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class ShowGuestsViewModel : ViewModelBase
    {
        private readonly GuestService _guestService;
        private readonly CheckPointService _checkPointService;
        public ObservableCollection<Guest> Guests { get; set; }
        public Tour SelectedTour { get; set; }
        public Guest SelectedGuest { get; set; }
        public List<CheckPoint> CheckPoints { get; set; }
        public ObservableCollection<Tuple<string, string>> FilteredGuests { get; set; }
        public ICollectionView GuestsListView { get; set; }

        public ShowGuestsViewModel(Tour selectedTour,Guest selectedGuest)
        {
            SelectedGuest = selectedGuest;
            SelectedTour = selectedTour;
            _guestService = new GuestService(Injector.CreateInstance<IGuestRepository>());
            _checkPointService = new CheckPointService(Injector.CreateInstance<ICheckPointRepository>());

            FilteredGuests = new ObservableCollection<Tuple<string, string>>(); // obrisi prethodne podatke
            var checkPoints = _checkPointService.GetAll();
            var guests = _guestService.FindByTourId(selectedTour.Id);
            var filteredGuests = new List<Tuple<string, string>>();

            foreach (var guest in guests)
            {
                var guestsOnTour = _guestService.GetGuestsOnTour(guest, selectedTour, checkPoints);
                foreach (var tuple in guestsOnTour)
                {
                    // Only add tuple if it is not already in filteredGuests
                    if (!filteredGuests.Any(t => t.Item1 == tuple.Item1 && t.Item2 == tuple.Item2))
                    {
                        filteredGuests.Add(tuple);
                    }

                }
            }
           

            FilteredGuests = new ObservableCollection<Tuple<string, string>>(filteredGuests);

            CollectionViewSource guestsListViewSource = new CollectionViewSource();
            guestsListViewSource.Source = FilteredGuests;
            GuestsListView = guestsListViewSource.View;

            showReviewsCommand = new RelayCommand(Execute_ShowReviews,CanExecute_Command);
        }


        public event PropertyChangedEventHandler? PropertyChanged;

       


        private RelayCommand showReview;
        public RelayCommand showReviewsCommand
        {
            get => showReview;
            set
            {
                if (value != showReview)
                {
                    showReview = value;
                    OnPropertyChanged();
                }

            }
        }
        private bool CanExecute_Command(object parameter)
        {
            return true;
        }


        private void Execute_ShowReviews(object sender)
        {
            ShowTourReviewView showTourReviewsView = new ShowTourReviewView(SelectedGuest);
            showTourReviewsView.Show();
        }
    }
}
