using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.WPF.ViewModel
{
    public class CancelRenovationViewModel : ViewModelBase
    {
        public AccommodationRenovation SelectedRenovation { get; set; }
        public ScheduledRenovationsCancellationViewModel _scheduledRenovationsView { get; set; }
        public AccommodationRenovationService _renovationService { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }
        public Action CloseAction { get; set; }

        private string _accommodationName;

        public string AccommodationName
        {
            get => _accommodationName;
            set
            {
                if (value != _accommodationName)
                {
                    _accommodationName = value;
                    OnPropertyChanged();
                }
            }
        }
        
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
        
        public CancelRenovationViewModel(AccommodationRenovation selectedRenovation, ScheduledRenovationsCancellationViewModel scheduledRenovationsCancellationViewModel)
        {
            InitializeCommands();
            SelectedRenovation = selectedRenovation;
            _scheduledRenovationsView = scheduledRenovationsCancellationViewModel;
            _renovationService = new AccommodationRenovationService(Injector.CreateInstance<IAccommodationRenovationRepository>());

            AccommodationName = SelectedRenovation.Accommodation.Name;
            StartDate = SelectedRenovation.StartDate;
            EndDate = SelectedRenovation.EndDate;
        }

        private void InitializeCommands()
        {
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
            ConfirmCommand = new RelayCommand(Execute_ConfirmCommand, CanExecute_Command);
        }

        private void Execute_ConfirmCommand(object obj)
        {
            _renovationService.Delete(SelectedRenovation);

            _scheduledRenovationsView.FutureRenovations.Remove(SelectedRenovation);
            CloseAction();
        }
        private void Execute_CancelCommand(object obj)
        {
            CloseAction();
        }
        private bool CanExecute_Command(object arg)
        {
            return true;
        }
    }
}
