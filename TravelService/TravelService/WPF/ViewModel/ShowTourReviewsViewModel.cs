using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class ShowTourReviewsViewModel : ViewModelBase
    {
        public NavigationService NavigationService;
        public ShowTourReviewView ShowTourReviewView;
        public Action CloseAction { get; set; }
        public Guest SelectedGuest { get; set; }
        public List<TourReview> TourReviews { get; set; }
        public List<TourReview> Reviews { get; set; }
        public TourReview SelectedTourReview { get; set; }
        private readonly TourReviewService _tourReviewService;
        public RelayCommand CancelCommand { get; set; }

        public ShowTourReviewsViewModel(Guest selectedGuest, TourReview selectedTourReview, NavigationService navigationService, ShowTourReviewView showTourReviewView)
        {
            NavigationService = navigationService;
            ShowTourReviewView = showTourReviewView;
            SelectedTourReview = selectedTourReview;
            SelectedGuest = selectedGuest;
            _tourReviewService = new TourReviewService(Injector.CreateInstance<ITourReviewRepository>());
            TourReviews = _tourReviewService.GetAll();
            Reviews = _tourReviewService.FindGuestsTourReviews(TourReviews, SelectedGuest);

            showReportCommand = new RelayCommand(Execute_Report, CanExecute_Command);
            CancelCommand = new RelayCommand(Cancel_Command,CanExecute_Command);    


        }

        private RelayCommand showReport;
        public RelayCommand showReportCommand
        {
            get => showReport;
            set
            {
                if (value != showReport)
                {
                    showReport = value;
                    OnPropertyChanged();
                }

            }
        }
        private string _confirmationMessage;
        public string ConfirmationMessage
        {
            get { return _confirmationMessage; }
            set
            {
                _confirmationMessage = value;
                OnPropertyChanged(nameof(ConfirmationMessage));
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        private void Cancel_Command(object sender)
            {
             NavigationService.GoBack();
            }

        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

        private  void Execute_Report(object sender)
        {
            if (SelectedTourReview != null)
            {
              NavigationService.Navigate(new ReportView(SelectedTourReview,SelectedGuest,NavigationService));
            }
            else
            {
                ErrorMessage="Choose review to report!";
            }
        }

    }
}