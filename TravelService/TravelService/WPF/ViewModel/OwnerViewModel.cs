using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.View;
using TravelService.WPF.Services;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class OwnerViewModel : ViewModelBase
    {
        public Owner Owner { get; set; }

        public NavigationService _navigationService;

        public ForumService _forumService;
        public Action CloseAction { get; set; }
        public OwnerView OwnerView { get; set; }
        public Frame MainFrame { get; set; }
        public RelayCommand AddAccommodationCommand { get; set; }
        public RelayCommand GuestRatingCommand { get; set; }
        public RelayCommand ReviewSelectionCommand { get; set; }
        public RelayCommand ReservationRequestsCommand { get; set; }
        public RelayCommand LogOutCommand { get; set; }
        public RelayCommand ShowProfileCommand { get; set; }    
        public RelayCommand ScheduleRenovationCommand { get; set; }    
        public RelayCommand ShowRenovationsCommand { get; set; }    
        public RelayCommand ShowStatisticsCommand { get; set; }
        public RelayCommand ShowForumsCommand { get; set; }


        private object _currentViewModel;

        public object CurrentViewModel 
        {
            get => _currentViewModel;
            set
            {
                if (value != _currentViewModel)
                {
                    _currentViewModel = value;
                    OnPropertyChanged();
                }
            }

        }

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

        public OwnerViewModel(Owner owner, OwnerView ownerView)
        {
            this.Owner = owner;
            OwnerView = ownerView;
            _forumService = new ForumService(Injector.CreateInstance<IForumRepository>());
           
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
            ShowForumsCommand = new RelayCommand(Execute_ShowForumsCommand, CanExecute_Command);
        }
        private void Execute_ShowStatisticsCommand(object obj)
        {
            AccommodationStatisticsView accommodationStatisticsView = new AccommodationStatisticsView(Owner);
            OwnerWindow ownerWindow = Window.GetWindow(OwnerView) as OwnerWindow ?? new(Owner);
            ownerWindow?.SwitchToPage(accommodationStatisticsView);
        }
        private void Execute_ShowForumsCommand(object obj)
        {
            ForumSelectionView forumSelectionView = new ForumSelectionView(Owner);
            OwnerWindow ownerWindow = Window.GetWindow(OwnerView) as OwnerWindow ?? new(Owner);
            ownerWindow?.SwitchToPage(forumSelectionView);
        }
        private void Execute_ShowRenovationsCommand(object obj)
        {
            ScheduledRenovationsCancellationView scheduledRenovationsCancellationView = new ScheduledRenovationsCancellationView(Owner);
            OwnerWindow ownerWindow = Window.GetWindow(OwnerView) as OwnerWindow ?? new(Owner);
            ownerWindow?.SwitchToPage(scheduledRenovationsCancellationView);
        }
        private void Execute_ScheduleRenovationCommand(object obj)
        {
            RenovationSelectionView renovationSelectionView = new RenovationSelectionView(Owner);
            OwnerWindow ownerWindow = Window.GetWindow(OwnerView) as OwnerWindow ?? new(Owner);
            ownerWindow?.SwitchToPage(renovationSelectionView);
        }
        private void Execute_ShowProfileCommand(object obj)
        {
            OwnerProfileView ownerProfileView = new OwnerProfileView(Owner);
            OwnerWindow ownerWindow = Window.GetWindow(OwnerView) as OwnerWindow ?? new(Owner);
            ownerWindow?.SwitchToPage(ownerProfileView);
        }
        private void Execute_AddAccommodationCommand(object obj)
        {
            AddAccommodation addAccommodation = new AddAccommodation(Owner, null);
            OwnerWindow ownerWindow = Window.GetWindow(OwnerView) as OwnerWindow ?? new(Owner);
            ownerWindow?.SwitchToPage(addAccommodation);
        }

        private void Execute_GuestRatingCommand(object obj)
        {
            GuestRatingOverview ratingOverview = new GuestRatingOverview(Owner);
            OwnerWindow ownerWindow = Window.GetWindow(OwnerView) as OwnerWindow ?? new(Owner);
            ownerWindow?.SwitchToPage(ratingOverview);
        }

        private void Execute_ReviewSelectionCommand(object obj)
        {
            ReviewsSelectionView reviewSelection = new ReviewsSelectionView(Owner);
            OwnerWindow ownerWindow = Window.GetWindow(OwnerView) as OwnerWindow ?? new(Owner);
            ownerWindow?.SwitchToPage(reviewSelection);
        }

        private void Execute_ReservationRequestsCommand(object obj)
        {
            MovingReservationRequestsView movingReservationRequests = new MovingReservationRequestsView();
            OwnerWindow ownerWindow = Window.GetWindow(OwnerView) as OwnerWindow ?? new(Owner);
            ownerWindow?.SwitchToPage(movingReservationRequests);
        }

        private void Execute_LogOutCommand(object obj)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            OwnerWindow ownerWindow = Window.GetWindow(OwnerView) as OwnerWindow ?? new(Owner);
            ownerWindow.Close();
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }
    }
}
