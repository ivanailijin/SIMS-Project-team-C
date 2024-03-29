﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.View;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class OwnerViewModel : ViewModelBase
    {
        public Owner Owner { get; set; }

        public Action CloseAction { get; set; }
        public RelayCommand AddAccommodationCommand { get; set; }
        public RelayCommand GuestRatingCommand { get; set; }
        public RelayCommand ReviewSelectionCommand { get; set; }
        public RelayCommand ReservationRequestsCommand { get; set; }
        public RelayCommand LogOutCommand { get; set; }
        public RelayCommand ShowProfileCommand { get; set; }    
        public RelayCommand ScheduleRenovationCommand { get; set; }    
        public RelayCommand ShowRenovationsCommand { get; set; }    
        public RelayCommand ShowStatisticsCommand { get; set; }    


        private bool _isSuperOwner;
        public bool IsSuperOwner
        {
            get => _isSuperOwner;
            set
            {
                if (value != _isSuperOwner)
                {
                    _isSuperOwner = value;
                    OnPropertyChanged(nameof(IsSuperOwner));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public OwnerViewModel(Owner owner)
        {
            this.Owner = owner;
            IsSuperOwner = owner.SuperOwner;
            InitializeCommands();
        }
        private void InitializeCommands()
        {
            AddAccommodationCommand = new RelayCommand(Execute_AddAccommodationCommand, CanExecute_Command);
            GuestRatingCommand = new RelayCommand(Execute_GuestRatingCommand, CanExecute_Command);
            ReviewSelectionCommand = new RelayCommand(Execute_ReviewSelectionCommand, CanExecute_Command);
            ReservationRequestsCommand = new RelayCommand(Execute_ReservationRequestsCommand, CanExecute_Command);
            ShowProfileCommand = new RelayCommand(Execute_ShowProfileCommand, CanExecute_Command);
            ScheduleRenovationCommand = new RelayCommand(Execute_ScheduleRenovationCommand, CanExecute_Command);
            LogOutCommand = new RelayCommand(Execute_LogOutCommand, CanExecute_Command);
            ShowRenovationsCommand = new RelayCommand(Execute_ShowRenovationsCommand, CanExecute_Command);
            ShowStatisticsCommand = new RelayCommand(Execute_ShowStatisticsCommand, CanExecute_Command);
        }
        private void Execute_ShowStatisticsCommand(object obj)
        {
            AccommodationStatisticsView accommodationStatisticsView = new AccommodationStatisticsView(Owner);
            accommodationStatisticsView.Show();
        }
        private void Execute_ShowRenovationsCommand(object obj)
        {
            ScheduledRenovationsCancellationView scheduledRenovationsCancellationView = new ScheduledRenovationsCancellationView(Owner);
            scheduledRenovationsCancellationView.Show();
        }
        private void Execute_ScheduleRenovationCommand(object obj)
        {
            RenovationSelectionView renovationSelectionView = new RenovationSelectionView(Owner);
            renovationSelectionView.Show();
        }
        private void Execute_ShowProfileCommand(object obj)
        {
            OwnerProfileView ownerProfileView = new OwnerProfileView(Owner);
            ownerProfileView.Show();
        }
        private void Execute_AddAccommodationCommand(object obj)
        {
            AddAccommodation addAccommodation = new AddAccommodation(Owner);
            addAccommodation.Show();
        }

        private void Execute_GuestRatingCommand(object obj)
        {
            GuestRatingOverview ratingOverview = new GuestRatingOverview(Owner);
            ratingOverview.Show();
        }

        private void Execute_ReviewSelectionCommand(object obj)
        {
            ReviewsSelectionView reviewSelection = new ReviewsSelectionView(Owner);
            reviewSelection.Show();
        }

        private void Execute_ReservationRequestsCommand(object obj)
        {
            MovingReservationRequestsView movingReservationRequests = new MovingReservationRequestsView();
            movingReservationRequests.Show();
        }

        private void Execute_LogOutCommand(object obj)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            CloseAction();
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }
    }
}
