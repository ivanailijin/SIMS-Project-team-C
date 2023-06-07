using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class ComplexRequestsViewModel : ViewModelBase
    {
        public Frame PopupFrame { get; set; }
        public TourRequest SelectedTour { get; set; }
        public ComplexRequests ComplexRequests { get; set; }    
        public Dates Dates { get; set; }

        public NavigationService NavigationService;
        public AcceptingComplexReq AcceptingComplexReq { get; set; }
        public ComplexTourRequest SelectedComplex { get; set; }
        public Guide Guide { get; set; }
        public Guest2 Guest2 { get; set; }
        public RelayCommand Accept { get; set; }

        private readonly ComplexTourRequestService _complexService;
        private readonly TourRequestService _tourRequestService;

        public ObservableCollection<DateTime> AvailableDates { get; set; }
        public DateTime SelectedDate { get; set; }


        private ObservableCollection<TourRequest> _tourRequests;
        public ObservableCollection<TourRequest> TourRequests
        {
            get => _tourRequests;
            set
            {
                if (value != _tourRequests)
                {
                    _tourRequests = value;
                    OnPropertyChanged();
                    List<TourRequest> tourRequest = _tourRequests.ToList();
                }
            }
        }
        private ObservableCollection<ComplexTourRequest> _complextourRequests;
        public ObservableCollection<ComplexTourRequest> ComplexTourRequests
        {
            get => _complextourRequests;
            set
            {
                if (value != _complextourRequests)
                {
                    _complextourRequests = value;
                    OnPropertyChanged();
                    List<ComplexTourRequest> complextourRequest = _complextourRequests.ToList();
                }
            }
        }

        public ComplexRequestsViewModel(Guide guide,TourRequest selectedReq, ComplexRequests complexRequestss,ComplexTourRequest selectedComplex,NavigationService navigationSerivce)
        {
            Guide = guide;
            SelectedTour = selectedReq;
            ComplexRequests = complexRequestss;
            NavigationService = navigationSerivce;  
            _complexService = new ComplexTourRequestService(Injector.CreateInstance<IComplexTourRequestRepository>());
            _tourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>());

            
            SelectedComplex = selectedComplex;
            List<TourRequest> allTourRequests = new List<TourRequest>(_tourRequestService.GetAll());
            List<ComplexTourRequest> complexRequests = new List<ComplexTourRequest>(_complexService.GetAll());
            List<ComplexTourRequest> guestsComplexRequests = new List<ComplexTourRequest>(_complexService.GetGuidesComplexRequests(complexRequests));
            List<TourRequest> tourRequests = new List<TourRequest>(_complexService.FindTourRequests(SelectedComplex, guestsComplexRequests));
            TourRequests = new ObservableCollection<TourRequest>(tourRequests);
            AvailableDates = new ObservableCollection<DateTime>();
            SelectedDate = DateTime.MinValue;
            Accept = new RelayCommand(AcceptCommand, CanExecute_Command);

            PopupFrame = complexRequestss.MyPopupFrame;
            OnPropertyChanged(nameof(Accept)); // Dodajte ovu liniju da biste obavijestili vezu o promjeni svojstva

            // Dodajte sljedeći kod ispod toga
            var acceptButton = PopupFrame.FindName("AcceptButton") as Button;
            acceptButton.Command = Accept;

        }


        private void AcceptCommand(object sender)

        {
            NavigationService.Navigate(new Dates(Guide,SelectedTour,SelectedComplex, NavigationService));
          



        }
        private void OpenPopupPage(Page Dates)
        {
            PopupFrame.Navigate(Dates);
            PopupFrame.Visibility = System.Windows.Visibility.Visible;

        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }




    }
}
