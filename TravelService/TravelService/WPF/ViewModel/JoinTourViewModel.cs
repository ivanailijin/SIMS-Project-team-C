using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class JoinTourViewModel : ViewModelBase
    {

        private readonly TourService _tourService;

        private readonly CheckPointService _checkpointService;
        public static List<CheckPoint> FilteredCheckPoint { get; set; }
        public ObservableCollection<Tour> Tours { get; set; }
        public Guest2 Guest2 { get; set; }
        public Tour SelectedTour { get; set; }
        public Action CloseAction { get; set; }
        public static List<CheckPoint> CheckPoints { get; set; }
        private RelayCommand _joinTourCommand;
        public RelayCommand JoinTourCommand
        {
            get => _joinTourCommand;
            set
            {
                if (value != _joinTourCommand)
                {
                    _joinTourCommand = value;
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
        public JoinTourViewModel(Tour selectedTour, Guest2 guest2)
        {
            _tourService = new TourService(Injector.CreateInstance<ITourRepository>());
            _checkpointService = new CheckPointService(Injector.CreateInstance<ICheckPointRepository>());
            SelectedTour = selectedTour;
            Guest2 = guest2;
            List<Tour> tours = new List<Tour>(_tourService.GetAll());
            Tours = new ObservableCollection<Tour>(tours);
            CheckPoints = new List<CheckPoint>(_checkpointService.GetAll());
            FilteredCheckPoint = _tourService.ShowListCheckPointList(SelectedTour.Id, Tours.ToList(), CheckPoints);

            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
            JoinTourCommand = new RelayCommand(Execute_JoinTourCommand, CanExecute_Command);
        }
        private bool CanExecute_Command(object parameter)
        {
            return true;
        }
        private void Execute_JoinTourCommand(object sender)
        {
            SecondGuestView secondGuestView = new SecondGuestView(Guest2);
            secondGuestView.Show();
            CloseAction();
        }
        private void Execute_VoucherViewCommand(object sender)
        {
            GuestsVouchersView guestsVouchersView = new GuestsVouchersView(Guest2);
            guestsVouchersView.Show();
        }
        private void Execute_HomePageCommand(object sender)
        {
            SecondGuestView secondGuestView = new SecondGuestView(Guest2);
            secondGuestView.Show();
            CloseAction();
        }
        private void Execute_CancelCommand(object sender)
        {
            CloseAction();
        }
    }
}
