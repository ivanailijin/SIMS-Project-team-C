using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class GuestsComplexRequestsViewModel : ViewModelBase
    {
        public Guest2 Guest2 { get; set; }
        public Action CloseAction { get; set; }
        public ObservableCollection<ComplexTourRequest> GuestsComplexRequests { get; set; }
        public List<TourRequest> ComplexTourRequests { get; set; }
        public ComplexTourRequest SelectedComplexRequest { get; set; }

        private readonly ComplexTourRequestService _complexTourRequestService;

        private RelayCommand _showComplexRequestCommand;
        public RelayCommand ShowComplexRequestCommand
        {
            get => _showComplexRequestCommand;
            set
            {
                if (value != _showComplexRequestCommand)
                {
                    _showComplexRequestCommand = value;
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
        private List<TourRequest> _tourRequests;

        public List<TourRequest> GuestsTourRequests
        {
            get { return _tourRequests; }
            set
            {
                _tourRequests = value;
                OnPropertyChanged(nameof(GuestsTourRequests));
            }
        }
        public GuestsComplexRequestsViewModel(Guest2 guest2, ComplexTourRequest selectedComplexRequest) 
        {
            _complexTourRequestService = new ComplexTourRequestService(Injector.CreateInstance<IComplexTourRequestRepository>());

            Guest2 = guest2;
            SelectedComplexRequest = selectedComplexRequest;
            List<ComplexTourRequest> complexRequests = new List<ComplexTourRequest>(_complexTourRequestService.GetAll());
            List<ComplexTourRequest> guestsComplexRequests = new List<ComplexTourRequest>(_complexTourRequestService.FindValidComplexRequests(Guest2.Id, complexRequests));
            GuestsComplexRequests = new ObservableCollection<ComplexTourRequest>(guestsComplexRequests);
            ShowComplexRequestCommand = new RelayCommand(Execute_ShowComplexRequestCommand, CanExecute_Command);
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);

        }
        private bool CanExecute_Command(object parameter)
        {
            return true;
        }
        private void Execute_ShowComplexRequestCommand(object sender)
        {
            ShowRequestListView showRequestListView = new ShowRequestListView(Guest2, SelectedComplexRequest);
            showRequestListView.Show();
            CloseAction();
        }
        private void Execute_CancelCommand(object sender)
        {
            CloseAction();
        }
    }
}
