using System;
using System.Collections.Generic;
using System.Windows.Documents;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class AddComplexTourRequestViewModel : ViewModelBase
    {
        public Guest2 Guest2 { get; set; }
        public Action CloseAction { get; set; }
        public List<TourRequest> TourRequests { get; set; }

        private bool _isForwarded = true;
        public bool IsForwarded
        {
            get { return _isForwarded; }
            set
            {
                _isForwarded = value;
            }
        }
        private RelayCommand _addRequestCommand;
        public RelayCommand AddRequestCommand
        {
            get => _addRequestCommand;
            set
            {
                if (value != _addRequestCommand)
                {
                    _addRequestCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        public AddComplexTourRequestViewModel(Guest2 guest2, bool isForwarded, List<TourRequest> tourRequests)
        {
            //_tourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>());

            Guest2 = guest2;
            IsForwarded = isForwarded;
            TourRequests = tourRequests;
            AddRequestCommand = new RelayCommand(Execute_AddRequestCommand, CanExecute_Command);

        }
        private bool CanExecute_Command(object parameter)
        {
            return true;
        }
        private void Execute_AddRequestCommand(object sender)
        {
            AddTourRequestView addTourRequestView = new AddTourRequestView(Guest2, IsForwarded, TourRequests);
            addTourRequestView.IsForwarded = IsForwarded;
            addTourRequestView.Show();
            CloseAction();
        }
    }
}
