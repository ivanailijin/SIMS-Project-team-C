using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class ChooseRequestTypeViewModel : ViewModelBase
    {
        public Guest2 Guest2 { get; set; }
        public Action CloseAction { get; set; }
        public ObservableCollection<TourRequest> TourRequests { get; set; }
        public  TourRequest  TourRequest  { get; set; }

        private RelayCommand _singleRequestCommand;
        public RelayCommand SingleRequestCommand
        {
            get => _singleRequestCommand;
            set
            {
                if (value != _singleRequestCommand)
                {
                    _singleRequestCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        private RelayCommand _complexRequestCommand;
        public RelayCommand ComplexRequestCommand
        {
            get => _complexRequestCommand;
            set
            {
                if (value != _complexRequestCommand)
                {
                    _complexRequestCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isForwarded = false;
        public bool IsForwarded
        {
            get { return _isForwarded; }
            set
            {
                _isForwarded = value;

            }
        }
        public ChooseRequestTypeViewModel(Guest2 guest2) 
        {
            Guest2 = guest2;
            TourRequests = new ObservableCollection<TourRequest>();
            SingleRequestCommand = new RelayCommand(Execute_SingleRequestCommand, CanExecute_Command);
            ComplexRequestCommand = new RelayCommand(Execute_ComplexRequestCommand, CanExecute_Command);
        }
        private bool CanExecute_Command(object parameter)
        {
            return true;
        }
        private void Execute_SingleRequestCommand(object sender)
        {
            AddTourRequestView addTourRequestView = new AddTourRequestView(Guest2,IsForwarded,TourRequests);
            addTourRequestView.IsForwarded = false;
            addTourRequestView.Show();
            CloseAction();
        }
        private void Execute_ComplexRequestCommand(object sender)
        {
            AddComplexTourRequestView addComplexTourRequestView = new AddComplexTourRequestView(Guest2, TourRequests);
            addComplexTourRequestView.Show();
            CloseAction();
        }
    }
}
