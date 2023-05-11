using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.WPF.ViewModel
{
    public class GuestsRequestsViewModel : ViewModelBase
    {
        private readonly TourRequestService _tourRequestService;
        public ObservableCollection<TourRequest> GuestsRequests{ get; set; }

        public Guest2 Guest2 { get; set; }
        public Action CloseAction { get; set; }
        public GuestsRequestsViewModel(Guest2 guest2) 
        {
            _tourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>());

            Guest2 = guest2;
            List<TourRequest> tourRequests = new List<TourRequest>(_tourRequestService.GetAll());
            List<TourRequest> guestsRequests = new List<TourRequest>(_tourRequestService.FindGuestsRequests(tourRequests,guest2.Id));
            GuestsRequests = new ObservableCollection<TourRequest>(guestsRequests);

        }
    }
}
