using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class RenovationSchedulingViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public AccommodationRenovationService _renovationService;
        public Action CloseAction { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand HomeCommand { get; set; }
        public RelayCommand ScheduleRenovationCommand { get; set; }
        public RelayCommand ShowAvailableDatesCommand { get; set; }
        public RenovationSchedulingView RenovationSchedulingView { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public Tuple<DateTime, DateTime> SelectedAvailableDatePair { get; set; }

        public Owner Owner { get; set; }


        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (value != _startDate)
                {
                    _startDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (value != _endDate)
                {
                    _endDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<Tuple<DateTime, DateTime>> _availableDatesPair;
        public ObservableCollection<Tuple<DateTime, DateTime>> AvailableDatesPair 
        {
            get => _availableDatesPair;
            set
            {
                if (value != _availableDatesPair)
                {
                    _availableDatesPair = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _renovationDuration;
        public int RenovationDuration
        {
            get => _renovationDuration;
            set
            {
                if (value != _renovationDuration)
                {
                    _renovationDuration = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _renovationDescription;
        public string RenovationDescription
        {
            get => _renovationDescription;
            set
            {
                if (value != _renovationDescription)
                {
                    _renovationDescription = value;
                    OnPropertyChanged();
                }
            }
        }

        public RenovationSchedulingViewModel(Owner owner, Accommodation selectedAccommodation, RenovationSchedulingView renovationSchedulingView)
        {
            InitializeCommands();
            this.Owner = owner;
            SelectedAccommodation = selectedAccommodation;
            AvailableDatesPair = new ObservableCollection<Tuple<DateTime, DateTime>>();
            _renovationService = new AccommodationRenovationService(Injector.CreateInstance<IAccommodationRenovationRepository>());
            RenovationSchedulingView = renovationSchedulingView;
        }
        private void InitializeCommands()
        {
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
            HomeCommand = new RelayCommand(Execute_HomeCommand, CanExecute_Command);
            ScheduleRenovationCommand = new RelayCommand(Execute_ScheduleRenovationCommand, CanExecute_Command);
            ShowAvailableDatesCommand = new RelayCommand(Execute_ShowAvailableDatesCommand, CanExecute_Command);
        }
        private void Execute_ShowAvailableDatesCommand(object obj)
        {
            if (StartDate < EndDate)
            {
               AvailableDatesPair = new ObservableCollection<Tuple<DateTime, DateTime>>(_renovationService.FindAvailableDates(StartDate, EndDate, RenovationDuration, SelectedAccommodation));
            }
            else
            {
                MessageBox.Show("Pocetni datum mora biti manji od krajnjeg!");
            }
        }
        private void Execute_ScheduleRenovationCommand(object obj)
        {
            if (SelectedAvailableDatePair != null)
            {
                AccommodationRenovation accommodationRenovation = new AccommodationRenovation(SelectedAccommodation.Id, Owner.Id, SelectedAvailableDatePair.Item1, SelectedAvailableDatePair.Item2, RenovationDescription);
                _renovationService.Save(accommodationRenovation);
                RenovationSchedulingView.GoBack();
            }
            else
            {
                MessageBox.Show("Morate izabrati opseg datuma kada zelite da zakazete renoviranje!");
            }
        }
        private void Execute_CancelCommand(object obj)
        {
            RenovationSchedulingView.GoBack();
        }
        private void Execute_HomeCommand(object obj)
        {
            OwnerView ownerView = new OwnerView(Owner);
            OwnerWindow ownerWindow = Window.GetWindow(RenovationSchedulingView) as OwnerWindow;
            ownerWindow?.SwitchToPage(ownerView);
        }
        private bool CanExecute_Command(object arg)
        {
            return true;
        }
    }
}
