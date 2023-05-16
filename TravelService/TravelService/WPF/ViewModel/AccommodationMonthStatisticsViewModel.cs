using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TravelService.Application.UseCases;
using TravelService.Commands;
using TravelService.Domain.Model;

namespace TravelService.WPF.ViewModel
{
    public class AccommodationMonthStatisticsViewModel : ViewModelBase
    {
        public AccommodationStatisticsService _statisticsService { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public AccommodationYearStatistics SelectedYear { get; set; }
        public int BusiestMonth { get; set; }
        public ObservableCollection<AccommodationMonthStatistics> MonthStatistics { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand ShowMonthStatistics { get; set; }
        public Action CloseAction { get; set; }

        public AccommodationMonthStatisticsViewModel(Accommodation selectedAccommodation, AccommodationYearStatistics selectedYear)
        {
            InitializeCommands();
            SelectedAccommodation = selectedAccommodation;
            SelectedYear = selectedYear;
            _statisticsService = new AccommodationStatisticsService();
            MonthStatistics = new ObservableCollection<AccommodationMonthStatistics>(_statisticsService.GetAccommodationMonthStatistics(selectedAccommodation, selectedYear.Year));
            BusiestMonth = _statisticsService.GetBusiestMonth(SelectedAccommodation, SelectedYear.Year);
        }

        private void InitializeCommands()
        {
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
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
