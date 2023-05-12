using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.WPF.ViewModel
{
    public class AccommodationYearStatisticsViewModel : ViewModelBase
    {
        public AccommodationStatisticsService _statisticsService { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public ObservableCollection<AccommodationYearStatistics> YearStatistics { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand ShowMonthStatistics { get; set; }
        public Action CloseAction { get; set; }

        public AccommodationYearStatisticsViewModel(Accommodation selectedAccommodation)
        {
            InitializeCommands();
            SelectedAccommodation = selectedAccommodation;
            _statisticsService = new AccommodationStatisticsService();
            YearStatistics = new ObservableCollection<AccommodationYearStatistics>(_statisticsService.GetAccommodationYearStatistics(selectedAccommodation.Id));
        }

        private void InitializeCommands()
        {
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
            ShowMonthStatistics = new RelayCommand(Execute_ShowMonthStatistics, CanExecute_Command);
        }

        private void Execute_ShowMonthStatistics(object obj)
        {
            
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
