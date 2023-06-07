using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;


namespace TravelService.WPF.ViewModel
{
    public class DatesViewModel :ViewModelBase
    {

        public Frame PopupFrame { get; set; }

     
        public Dates Dates { get; set; }    
        public NavigationService NavigationService;
        public TourRequest SelectedTourRequest { get; set; }
        public List<Tour> ExistingTours { get; set; }
        public bool IsDateAvailable { get; set; }
        public RelayCommand ScheduleCommand { get; set; }
        private readonly TourRequestService _tourRequestService;
        private readonly NewTourNotificationService _newTourNotificationService;
        private readonly TourService _tourService;
        public Guide Guide { get; set; }
       
        public ComplexTourRequest SelectedComplex { get; set; }
        
        public List<DateTime> AvailableDates { get; set; }
        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
            }
        }

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

        public DatesViewModel(TourRequest selectedTourRequest, ComplexTourRequest selectedComplex, NavigationService navigationService, Dates dates, Guide guide)
        {
            SelectedComplex = selectedComplex;
            Guide = guide;
            NavigationService = navigationService;
            Dates = dates;
            SelectedTourRequest = selectedTourRequest;
            _tourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>());
            _tourService = new TourService(Injector.CreateInstance<ITourRepository>());
            _newTourNotificationService = new NewTourNotificationService(Injector.CreateInstance<INewTourNotificationRepository>());
            ExistingTours = new List<Tour>();
            ExistingTours.AddRange(_tourService.GetAll());

            AvailableDates = new List<DateTime>();

            foreach (var tourRequest in SelectedComplex.TourRequests)
                foreach(Tour tour in ExistingTours)
                {
                    {
                        List<DateTime> availableDates = GetAvailableDates( tour);
                        foreach (var date in availableDates)
                        {
                            if(AvailableDates.Contains(date))
                            {
                                continue;
                            }
                            AvailableDates.Add(date);
                        }
                    }
            }

            SelectedDate = DateTime.MinValue;
            ScheduleCommand = new RelayCommand(ScheduleTour, CanScheduleTour);

            PopupFrame = dates.MyPopupFrame;

            OnPropertyChanged(nameof(AvailableDates));
        }


        private List<DateTime> GetAvailableDates(Tour tour)
        {
            List<DateTime> availableDates = new List<DateTime>();

            // Retrieve the list of existing tours for the guide
            List<Tour> guideTours = _tourService.FindGuidesTours(Guide.Id);

            // Retrieve the existing tour parts for the current complex tour
            List<TourRequest> existingTourParts = SelectedComplex.TourRequests.ToList();

            // Retrieve the start and end dates for the current tour part
            DateTime tourPartStart = SelectedTourRequest.TourStart;
            DateTime tourPartEnd = SelectedTourRequest.TourEnd;

            // Iterate over the dates within the tour part range
            for (DateTime date = tourPartStart.Date; date <= tourPartEnd.Date; date = date.AddDays(1))
            {
                bool isNoConflict = guideTours
                    .Where(t => t != tour)
                    .All(t => date < t.TourStart.Date || date > t.TourEnd.Date);

                if (isNoConflict && !guideTours.Any(tp => tp.TourStart.Date == date) && !availableDates.Contains(date))
                {
                    availableDates.Add(date);
                }
            }

            return availableDates;
        }




        // Schedule the tour for the selected date
        private void ScheduleTour(object obj)
        {
            if (SelectedDate != null)
            {
                bool isDateAvailable = CheckIfDateIsAvailable(SelectedDate);

                if (isDateAvailable)
                {
                    // Update the tour date and mark it as accepted
                    SelectedTourRequest.TourStart = SelectedDate;
                    SelectedTourRequest.RequestApproved = APPROVAL.ACCEPTED;
                    _tourRequestService.Update(SelectedTourRequest);
                    ConfirmationMessage = "Tour scheduled successfully!";
                    ErrorMessage = ""; // Clear any previous error message
                }
                else
                {
                    ErrorMessage = "Selected date is not available. Please choose another date.";
                    ConfirmationMessage = ""; // Clear any previous confirmation message
                }
            }
            else
            {
                ErrorMessage = "Please select a date.";
                ConfirmationMessage = ""; // Clear any previous confirmation message
            }
        }

        // Check if the selected date is available
        private bool CheckIfDateIsAvailable(DateTime selectedDate)
        {
            foreach (Tour tour in ExistingTours)
            {
                List<DateTime> availableDates = GetAvailableDates(tour);

                if (!availableDates.Contains(selectedDate))
                {
                    return false;
                }
            }

            return true;
        }





        private bool CanScheduleTour(object obj)
        {
            return AvailableDates.Contains(SelectedDate);
        }




    }


}
