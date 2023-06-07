using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Navigation;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class ScheduleDateViewModel : ViewModelBase
    {
        public NavigationService NavigationService;
        public ScheduleDateView ScheduleDateView;
        public Guest2 Guest2 { get; set; }
        public Guide Guide { get; set; }    
        public Action CloseAction { get; set; }
        private readonly TourRequestService _tourRequestService;
        private readonly NewTourNotificationService _newTourNotificationService;
        private readonly TourService _tourService;
        public TourRequest SelectedTourRequest { get; set; }
        public List<Tour> ExistingTours { get; set; }
        public bool IsDateAvailable { get; set; }
        public RelayCommand ScheduleCommand { get; set; }

        private string _confirmationMessage;
        public string ConfirmationMessage
        {
            get { return _confirmationMessage; }
            set
            {
                _confirmationMessage = value;
                OnPropertyChanged(nameof(ConfirmationMessage));
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public ScheduleDateViewModel(TourRequest selectedTourRequest,NavigationService navigationService,ScheduleDateView scheduleDateView)
        {
            NavigationService = navigationService;
            ScheduleDateView = scheduleDateView;
            SelectedTourRequest = selectedTourRequest;
            _tourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>());
            _tourService = new TourService(Injector.CreateInstance<ITourRepository>());
            _newTourNotificationService = new NewTourNotificationService(Injector.CreateInstance<INewTourNotificationRepository>());
            ExistingTours = new List<Tour>();
            ExistingTours.AddRange(_tourService.GetAll());
            SelectedDate=DateTime.Now;
            IsDateAvailable = _tourRequestService.AvailabilityDate(ExistingTours, SelectedDate);
            ScheduleCommand = new RelayCommand(ScheduleTour, CanScheduleTour);

        }
        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }



        // Schedule the tour for the selected date
        private void ScheduleTour(object obj)
        {
            ErrorMessage = "You must select a date within the valid tour range.";
            // Check if the selected date is within the valid range
            if (SelectedTourRequest.TourStart <= SelectedDate && SelectedDate <= SelectedTourRequest.TourEnd && SelectedDate !=null)
            {
                // Schedule the tour for the selected date
                SelectedTourRequest.TourStart = SelectedDate;
                SelectedTourRequest.RequestApproved = APPROVAL.ACCEPTED;
                _tourRequestService.Update(SelectedTourRequest);
                NavigationService.Navigate(new AcceptingTourRequestView(Guide, SelectedTourRequest,NavigationService));
                ConfirmationMessage = "Tura je zakazana!";
               
                _newTourNotificationService.TourRequestAcceptedNotification(SelectedTourRequest);
                CloseAction();
            }
            else
            {
               ErrorMessage = "You must select a date within the valid tour range.";
            }
        }




        private bool CanScheduleTour(object obj)
        {
            return IsDateAvailable;
        }



        private void Execute_CancelCommand(object obj)
        {
            CloseAction();
        }
    }

}

