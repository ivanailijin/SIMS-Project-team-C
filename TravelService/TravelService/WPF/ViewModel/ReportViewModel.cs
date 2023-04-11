using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Application.UseCases;
using TravelService.Domain.Model;
using TravelService.Commands;
using TravelService.Domain.RepositoryInterface;
using TravelService.Application.Utils;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class ReportViewModel : ViewModelBase
    {
        private readonly TourReviewService _tourReviewService;
        public TourReview SelectedTourReview { get; set; }  
        public Guest SelectedGuest { get; set; } 

        public ReportViewModel(TourReview selectedTourReview,Guest selectedGuest)
        {
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
            ShowTourReviewView showTourReviewsView = new ShowTourReviewView(SelectedGuest, SelectedTourReview);
            showTourReviewsView.Show();

        }

        private void Execute_CancelCommand(object obj)
        {
            ShowTourReviewView showTourReviewsView = new ShowTourReviewView(SelectedGuest, SelectedTourReview);
            showTourReviewsView.Show();
        }
    }

    }

