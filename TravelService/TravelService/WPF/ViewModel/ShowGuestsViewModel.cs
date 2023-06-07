using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Navigation;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class ShowGuestsViewModel : ViewModelBase
    {
        public NavigationService NavigationService;
        public ShowGuestsView ShowGuestsView;
        public Action CloseAction { get; set; }
        private readonly GuestService _guestService;
        private readonly CheckPointService _checkPointService;
        public Tour SelectedTour { get; set; }
        public Guest SelectedGuest { get; set; }
        public List<CheckPoint> CheckPoints { get; set; }
        public List<Guest> GuestList { get; set; }
        public List<Guest> Guest { get; set; }
        public List<string> checkPoint { get; set; }
        public TourReview SelectedTourReview { get; set; }  
        public Guide Guide { get; set; }
        public RelayCommand CancelCommand { get; set; }

        public ShowGuestsViewModel(Tour selectedTour,Guest selectedGuest,NavigationService navigationService,ShowGuestsView showGuestsView)
        {
            ShowGuestsView = showGuestsView;
            NavigationService = navigationService;
            SelectedGuest = selectedGuest;
            SelectedTour = selectedTour;
            _guestService = new GuestService(Injector.CreateInstance<IGuestRepository>());
            _checkPointService = new CheckPointService(Injector.CreateInstance<ICheckPointRepository>());

            CheckPoints = _checkPointService.GetAll();
            Guest = _guestService.FindByTourId(selectedTour.Id);
            
            foreach (Guest guest in Guest)
            {
                GuestList = _guestService.GetGuestsOnTour(guest, selectedTour, CheckPoints);
                checkPoint = _guestService.FindCheckPointName(GuestList, CheckPoints);
            }
            
            showReviewsCommand = new RelayCommand(Execute_ShowReviews,CanExecute_Command);
              CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
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
            if (SelectedGuest == null)
            {
                MessageBox.Show("Please select guest first.");
                return;
            }
            NavigationService.Navigate(new ShowTourReviewView(SelectedGuest, SelectedTourReview, NavigationService));
           
        }
        private void Execute_CancelCommand(object obj)
        {

            NavigationService.GoBack();
        }
    }
    }
