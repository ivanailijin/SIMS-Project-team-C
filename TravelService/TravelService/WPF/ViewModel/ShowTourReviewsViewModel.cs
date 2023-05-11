using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class ShowTourReviewsViewModel : ViewModelBase
    {
        public Action CloseAction { get; set; }
        public Guest SelectedGuest { get; set; }
        public List<TourReview> TourReviews { get; set; }
        public List<TourReview> Reviews { get; set;}
        public TourReview SelectedTourReview { get; set; }
        private readonly TourReviewService _tourReviewService;
        public RelayCommand CancelCommand { get; set; }

        public ShowTourReviewsViewModel(Guest selectedGuest, TourReview selectedTourReview)
        {

                SelectedTourReview = selectedTourReview;
                SelectedGuest = selectedGuest;
                _tourReviewService = new TourReviewService(Injector.CreateInstance<ITourReviewRepository>());
                TourReviews = _tourReviewService.GetAll();
                Reviews = _tourReviewService.FindGuestsTourReviews(TourReviews, SelectedGuest);

            showReportCommand = new RelayCommand(Execute_Report, CanExecute_Command);
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
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
        private void Execute_CancelCommand(object obj)
        {

            CloseAction();
        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

        private  void Execute_Report(object sender)
        {
            if (SelectedTourReview != null)
            {
                ReportView reportView = new ReportView(SelectedTourReview,SelectedGuest);
                reportView.Show();
            }
            else
            {
                MessageBox.Show("Choose review to report!");
            }
        }

    }
}