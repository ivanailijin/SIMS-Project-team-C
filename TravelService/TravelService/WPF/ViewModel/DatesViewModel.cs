using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public string ErrorMessage { get; set; }
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
        
        public ObservableCollection<DateTime> AvailableDates { get; set; }
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



        public DatesViewModel(TourRequest selectedTourRequest, ComplexTourRequest selectedComplex, NavigationService navigationService, Dates dates,Guide guide)
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

            IsDateAvailable = _tourRequestService.AvailabilityDate(ExistingTours, SelectedDate);
            ScheduleCommand = new RelayCommand(ScheduleTour, CanScheduleTour);
            AvailableDates = new ObservableCollection<DateTime>();
            SelectedDate = DateTime.MinValue;
            GetAvailableDates();

            PopupFrame = dates.MyPopupFrame;

        }

        private List<DateTime> GetAvailableDates()
        {
            List<DateTime> availableDates = new List<DateTime>();

            // Dohvatite listu postojećih tura vodiča
            List<Tour> existingTours = _tourService.FindGuidesTours(Guide.Id);

            // Prođite kroz termine delova ture i provjerite dostupnost
            foreach (var tourRequest in SelectedComplex.TourRequests)
            {
                DateTime tourStart = tourRequest.TourStart;
                DateTime tourEnd = tourRequest.TourEnd;

                // Provjerite dostupnost za svaki dan unutar opsega datuma delova ture
                for (DateTime date = tourStart.Date; date <= tourEnd.Date; date = date.AddDays(1))
                {
                    if (_tourRequestService.AvailabilityDate(existingTours, date))
                    {
                        availableDates.Add(date);
                    }
                }
            }

            return availableDates;
        }



        // Schedule the tour for the selected date
        private void ScheduleTour(object obj)
        {
            if (SelectedDate != null)
            {
                List<DateTime> availableDates = GetAvailableDates();
                AvailableDates = new ObservableCollection<DateTime>(availableDates);

                if (!AvailableDates.Contains(SelectedDate))
                {
                    ErrorMessage = "Selected date is not available. Please choose another date.";
                    return;
                }

                // Ažurirajte datum ture i označite je kao prihvaćenu
                SelectedTourRequest.TourStart = SelectedDate;
                SelectedTourRequest.RequestApproved = APPROVAL.ACCEPTED;
                _tourRequestService.Update(SelectedTourRequest);

                NavigationService.Navigate(new AcceptingTourRequestView(Guide, SelectedTourRequest, NavigationService));
            }
            else
            {
                ErrorMessage = "Please select a date.";
            }
        }




        private bool CanScheduleTour(object obj)
        {
            return AvailableDates.Contains(SelectedDate);
        }




    }


}
