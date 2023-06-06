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
    public class AcceptingComplexREqViewModel : ViewModelBase
    {
        public Frame PopupFrame { get; set; }
        public TourRequest SelectedTour { get; set; }
        public ComplexRequests ComplexRequests { get; set; }    

        public NavigationService NavigationService;
        public AcceptingComplexReq AcceptingComplexReq { get; set; }
        public ComplexTourRequest SelectedComplex { get; set; }
        public Guide Guide { get; set; }
        public Guest2 Guest2 { get; set; }

        private readonly ComplexTourRequestService _complexService;
        private readonly TourRequestService _tourRequestService;




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

        public RelayCommand Accept { get; set; }

      



        public AcceptingComplexREqViewModel(TourRequest selectedReq,Guide guide, ComplexTourRequest selectedComplex, AcceptingComplexReq acceptingComplex, NavigationService navigationService)
        {
            SelectedTour = selectedReq;
           
            NavigationService = navigationService;
            Guide = guide;
            SelectedComplex = selectedComplex;
            AcceptingComplexReq = acceptingComplex;

            _complexService = new ComplexTourRequestService(Injector.CreateInstance<IComplexTourRequestRepository>());
            _tourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>());

            List <ComplexTourRequest> complexTourRequests = new List<ComplexTourRequest>(_complexService.GetAll());

            ComplexTourRequests = new ObservableCollection<ComplexTourRequest>(_complexService.GetAll());
            TourRequests = new ObservableCollection<TourRequest>(_tourRequestService.GetAll());

           Accept = new RelayCommand(AcceptCommand, CanExecute_Command);

            PopupFrame = acceptingComplex.MyPopupFrame;

        }
      

        private void AcceptCommand(object sender)

        {
            NavigationService.Navigate(new ComplexRequests(Guide,SelectedTour,SelectedComplex, NavigationService));
       



        }
     
        
        private bool CanExecute_Command(object arg)
        {
            return true;
        }
       

    }
}

