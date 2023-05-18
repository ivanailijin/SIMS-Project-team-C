using System;
using System.Collections.Generic;
using System.Windows;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class ScheduleDateViewModel : ViewModelBase
    {
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

        public ScheduleDateViewModel(TourRequest selectedTourRequest)
        {

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
            // Check if the selected date is within the valid range
            if (SelectedTourRequest.TourStart <= SelectedDate && SelectedDate <= SelectedTourRequest.TourEnd)
            {
                // Schedule the tour for the selected date
                SelectedTourRequest.TourStart = SelectedDate;
                SelectedTourRequest.RequestApproved = APPROVAL.ACCEPTED;
                _tourRequestService.Update(SelectedTourRequest);
                AcceptingTourRequestView acceptingTourRequestView = new AcceptingTourRequestView(Guide, SelectedTourRequest);
                acceptingTourRequestView.Show();
                _newTourNotificationService.TourRequestAcceptedNotification(SelectedTourRequest);
                CloseAction();
            }
            else
            {
                MessageBox.Show("You must select a date within the valid tour range.");
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

