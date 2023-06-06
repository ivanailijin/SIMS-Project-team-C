using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class AccommodationYearStatisticsViewModel : ViewModelBase
    {
        public AccommodationStatisticsService _statisticsService { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public AccommodationYearStatistics SelectedYear { get; set; }

        public Owner owner { get; set; }
        public AccommodationYearStatisticsView AccommodationYearStatisticsView { get; set; }
        public int BusiestYear { get; set; }
        public ObservableCollection<AccommodationYearStatistics> YearStatistics { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand HomeCommand { get; set; }
        public RelayCommand ShowMonthStatisticsCommand { get; set; }
        public Action CloseAction { get; set; }

        public AccommodationYearStatisticsViewModel(Accommodation selectedAccommodation, AccommodationYearStatisticsView accommodationYearStatisticsView, Owner owner)
        {
            InitializeCommands();
            SelectedAccommodation = selectedAccommodation;
            _statisticsService = new AccommodationStatisticsService();
            YearStatistics = new ObservableCollection<AccommodationYearStatistics>(_statisticsService.GetAccommodationYearStatistics(selectedAccommodation));
            BusiestYear = _statisticsService.GetBusiestYear(selectedAccommodation);
            AccommodationYearStatisticsView = accommodationYearStatisticsView;
            this.owner = owner;
        }

        private void InitializeCommands()
        {
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
            HomeCommand = new RelayCommand(Execute_HomeCommand, CanExecute_Command);
            ShowMonthStatisticsCommand = new RelayCommand(Execute_ShowMonthStatisticsCommand, CanExecute_Command);
        }

        private void Execute_ShowMonthStatisticsCommand(object obj)
        {
            if (SelectedYear != null)
            {
                AccommodationMonthStatisticsView accommodationMonthStatisticsView = new AccommodationMonthStatisticsView(SelectedAccommodation, SelectedYear);
                OwnerWindow ownerWindow = Window.GetWindow(AccommodationYearStatisticsView) as OwnerWindow;
                ownerWindow?.SwitchToPage(accommodationMonthStatisticsView);
            }
            else
            {
                MessageBox.Show("Niste izabrali godinu za prikaz statistike!");
            }
        }
        private void Execute_CancelCommand(object obj)
        {
            AccommodationYearStatisticsView.GoBack();
        }
        private void Execute_HomeCommand(object obj)
        {
            OwnerView ownerView = new OwnerView(owner);
            OwnerWindow ownerWindow = Window.GetWindow(AccommodationYearStatisticsView) as OwnerWindow;
            ownerWindow?.SwitchToPage(ownerView);
        }
        private bool CanExecute_Command(object arg)
        {
            return true;
        }
    }
}
