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
    public class ChooseRequestListViewModel : ViewModelBase
    {
        public Guest2 Guest2 { get; set; }
        public ComplexTourRequest SelectedComplexRequest { get; set; }
        public Action CloseAction { get; set; }

        private RelayCommand _singleRequestListCommand;
        public RelayCommand SingleRequestListCommand
        {
            get => _singleRequestListCommand;
            set
            {
                if (value != _singleRequestListCommand)
                {
                    _singleRequestListCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        private RelayCommand _complexRequestListCommand;
        public RelayCommand ComplexRequestListCommand
        {
            get => _complexRequestListCommand;
            set
            {
                if (value != _complexRequestListCommand)
                {
                    _complexRequestListCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        public ChooseRequestListViewModel(Guest2 guest2)
        {
            Guest2 = guest2;
            SingleRequestListCommand = new RelayCommand(Execute_SingleRequestListCommand, CanExecute_Command);
            ComplexRequestListCommand = new RelayCommand(Execute_ComplexRequestListCommand, CanExecute_Command);
        }
        private bool CanExecute_Command(object parameter)
        {
            return true;
        }
        private void Execute_SingleRequestListCommand(object sender)
        {
            GuestsRequestsView guestsRequestsView = new GuestsRequestsView(Guest2);
            guestsRequestsView.Show();
        }
        private void Execute_ComplexRequestListCommand(object sender)
        {
            GuestsComplexRequestsView guestsComplexRequestsView = new GuestsComplexRequestsView(Guest2, SelectedComplexRequest);
            guestsComplexRequestsView.Show();
        }
    }
}
