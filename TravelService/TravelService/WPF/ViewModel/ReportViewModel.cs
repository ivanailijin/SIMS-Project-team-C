using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Applications.UseCases;
using TravelService.Domain.Model;
using TravelService.Commands;
using TravelService.Domain.RepositoryInterface;
using TravelService.Applications.Utils;
using TravelService.WPF.View;
using System.Windows.Navigation;

namespace TravelService.WPF.ViewModel
{
    public class ReportViewModel : ViewModelBase
    {
        public NavigationService NavigationService;
        public ReportView ReportView;
        public Action CloseAction { get; set; }
        private readonly TourReviewService _tourReviewService;
        public TourReview SelectedTourReview { get; set; }  
        public Guest SelectedGuest { get; set; } 

        public ReportViewModel(TourReview selectedTourReview,Guest selectedGuest,NavigationService navigationService,ReportView reportView)
        {
            NavigationService = navigationService;
            ReportView = reportView;
            SelectedGuest = selectedGuest;
            SelectedTourReview = selectedTourReview;
            _tourReviewService = new TourReviewService(Injector.CreateInstance<ITourReviewRepository>());
            confirmCommand = new RelayCommand(Execute_ConfirmCommand, CanExecute_Command);
            cancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
        }



        private RelayCommand confirm;
        public RelayCommand confirmCommand
        {
            get => confirm;
            set
            {
                if (value != confirm)
                {
                    confirm = value;
                    OnPropertyChanged();
                }

            }
        }
        private RelayCommand cancel;
        public RelayCommand cancelCommand
        {
            get => cancel;
            set
            {
                if (value != cancel)
                {
                    cancel = value;
                    OnPropertyChanged();
                }

            }
        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
        }
        private void Execute_ConfirmCommand(object obj)
        {

            SelectedTourReview.Valid =false;
            _tourReviewService.Update(SelectedTourReview);
            NavigationService.Navigate( new ShowTourReviewView(SelectedGuest, SelectedTourReview,NavigationService));
          

        }

        private void Execute_CancelCommand(object obj)
        {
            SelectedTourReview.Valid = true;
            _tourReviewService.Update(SelectedTourReview);
            NavigationService.Navigate(new ShowTourReviewView(SelectedGuest, SelectedTourReview, NavigationService));
           
        }
    }

    }

