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

namespace TravelService.WPF.ViewModel
{
    public class ShowRequestListViewModel : ViewModelBase
    {
        public Action CloseAction { get; set; }
        public ComplexTourRequest SelectedComplexRequest { get; set; }
        public Guest2 Guest2 { get; set; }
        public ObservableCollection<TourRequest> TourRequests { get; set; }

        private readonly ComplexTourRequestService _complexTourRequestService;

        private readonly TourRequestService _tourRequestService;
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

        public ShowRequestListViewModel(Guest2 guest2, ComplexTourRequest selectedComplexRequest)
        {
            _complexTourRequestService = new ComplexTourRequestService(Injector.CreateInstance<IComplexTourRequestRepository>());
            _tourRequestService = new  TourRequestService(Injector.CreateInstance<ITourRequestRepository>());

            Guest2 = guest2;
            SelectedComplexRequest = selectedComplexRequest;
            List<TourRequest> allTourRequests= new List<TourRequest>(_tourRequestService.GetAll());
            List<TourRequest> tourRequests= new List<TourRequest>(_complexTourRequestService.FindTourRequests(SelectedComplexRequest,Guest2.Id,allTourRequests));
            TourRequests = new ObservableCollection<TourRequest>(tourRequests);
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);

        }
        private bool CanExecute_Command(object parameter)
        {
            return true;
        }
        private void Execute_CancelCommand(object sender)
        {
            CloseAction();
        }
    }
}
