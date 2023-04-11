using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.WPF.ViewModel
{
    public class ShowTourReviewsViewModel : ViewModelBase
    {
        public Guest SelectedGuest { get; set; }
        public List<TourReview> TourReviews { get; set; }

        private readonly TourReviewService _tourReviewService;

        public ShowTourReviewsViewModel(Guest selectedGuest)
        {
            if (selectedGuest == null || selectedGuest.Id == null)
            {
                TourReviews = new List<TourReview>();
            }
            else
            {

                SelectedGuest = selectedGuest;
                _tourReviewService = new TourReviewService(Injector.CreateInstance<ITourReviewRepository>());
                TourReviews = new List<TourReview>(_tourReviewService.FindTourReviewsByGuestId(selectedGuest.Id));

            }
        }



        

    }


    }

