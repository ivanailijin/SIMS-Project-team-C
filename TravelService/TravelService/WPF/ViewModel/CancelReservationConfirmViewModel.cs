using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Applications.UseCases;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Applications.Utils;
using TravelService.WPF.View;
using TravelService.Domain.RepositoryInterface;
using System.Windows;
using System.Collections.ObjectModel;

namespace TravelService.WPF.ViewModel
{
    public class CancelReservationConfirmViewModel : ViewModelBase
    {
        private readonly AccommodationReservationService _accommodationReservationService;
        private readonly AccommodationService _accommodationService;
        private ObservableCollection<AccommodationReservation> _activeReservations;
        public static ObservableCollection<AccommodationReservation> ActiveReservations { get; set; }
        public AccommodationReservation SelectedReservation { get; set; }
        public Guest1 Guest1 { get; set; }
        public Action CloseAction { get; set; }

        private RelayCommand _confirmCommand;
        public RelayCommand ConfirmCommand
        {
            get => _confirmCommand;
            set
            {
                if (value != _confirmCommand)
                {
                    _confirmCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _cancelCommand;
        public RelayCommand CancelCommand
        {
            get => _cancelCommand;
            set
            {
                if (value != _cancelCommand)
                {
                    _cancelCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        public CancelReservationConfirmViewModel(ObservableCollection<AccommodationReservation> activeReservations, AccommodationReservation selectedReservation, Guest1 guest1)
        {
            ActiveReservations = activeReservations;
            SelectedReservation = selectedReservation;
            Guest1 = guest1;

            _accommodationReservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
            _accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>());

            ConfirmCommand = new RelayCommand(Execute_ConfirmCommand, CanExecute_Command);
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

        private void Execute_ConfirmCommand(object sender)
        {
            _accommodationReservationService.CancelReservation(SelectedReservation);
            ActiveReservations.Remove(SelectedReservation);
            CloseAction();
        }

        private void Execute_CancelCommand(object sender)
        {
            CloseAction();
        }
    }
}
