using System;
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
            LogOutCommand = new RelayCommand(Execute_LogOutCommand, CanExecute_Command);
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
