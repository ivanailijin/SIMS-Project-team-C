using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class GuideHomePageViewModel : ViewModelBase
    {
        public Frame PopupFrame { get; set; }
        public GuideHomePageView GuideHomePageView { get; set; }
       
        public NavigationService NavigationService { get; set; }
        public TourRequest SelectedTourRequest { get; set; }
        private readonly GuideService _guideService;
        public Tour SelectedTour { get; set; }
        public bool Visibility { get; set; }
        public Guide Guide { get; set; }
        public List<Guest> Guests { get; set; }

        public Action CloseAction { get; set; }

        public RelayCommand LogOutCommand { get; set; }
        public RelayCommand ActiveToursCommand { get; set; }
        public RelayCommand AddTourCommand { get; set; }
        public RelayCommand CancelTourCommand { get; set; }
        public RelayCommand PastToursCommand { get; set; }
        public RelayCommand RequestsCommand { get; set; }
        public RelayCommand RequestsComplexCommand { get; set; }
        public RelayCommand BestTourCommand { get; set; }
        public RelayCommand AboutMeCommand { get; set; }
        public RelayCommand ThemeCommand { get; set; }
        public RelayCommand LanguageCommand { get; set; }
        public RelayCommand FutureToursCommand { get; set; }

        public RelayCommand Suggestion { get; set; }
        public Brush Background { get; set; }
        public Brush Foreground { get; set; }




        public GuideHomePageViewModel(NavigationService navigationService,Guide guide,GuideHomePageView guideHomePageView)
        {
            NavigationService = navigationService;
            this.Guide = guide;
            GuideHomePageView = guideHomePageView;
            _guideService = new GuideService(Injector.CreateInstance<IGuideRepository>());

            this.Username = guide.Username;


            LogOutCommand = new RelayCommand(Execute_LogOutCommand, CanExecute_Command);
            FutureToursCommand = new RelayCommand(Execute_Future, CanExecute_Command);
            AddTourCommand = new RelayCommand(Execute_AddTourCommand, CanExecute_Command);
            CancelTourCommand = new RelayCommand(Execute_CAncelTourCommand, CanExecute_Command);
            PastToursCommand = new RelayCommand(Execute_PastToursCommand, CanExecute_Command);
             RequestsCommand = new RelayCommand(Execute_RequestsCommand, CanExecute_Command);
            // RequestsComplexCommand = new RelayCommand(Execute_LogOutCommand, CanExecute_Command);
            BestTourCommand = new RelayCommand(Execute_BestTourCommand, CanExecute_Command);
            // AboutMeCommand = new RelayCommand(Execute_LogOutCommand, CanExecute_Command);
          
            LanguageCommand = new RelayCommand(Execute_LogOutCommand, CanExecute_Command);
            Suggestion = new RelayCommand(Execute_SuggestionCommand,CanExecute_Command);

            PopupFrame = guideHomePageView.MyPopupFrame;

            NavigationService.Navigate(new ActiveToursView(SelectedTour, NavigationService))
;        }

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }
        private void Execute_Future(object obk)
        {
            NavigationService.Navigate(new MyTours(SelectedTour, Guide, NavigationService));
        }
        private void Execute_SuggestionCommand(object obj)
        {
            var suggestion = new SuggestionForGuideView(Guide, NavigationService);
            OpenPopupPage(suggestion);
        }
        private void OpenPopupPage(Page SuggestionForGuideView)
        {
            PopupFrame.Navigate(SuggestionForGuideView);
            PopupFrame.Visibility = System.Windows.Visibility.Visible;

        }


        private void Execute_RequestsCommand(object obj)
        {
            NavigationService.Navigate(new AcceptingTourRequestView(Guide, SelectedTourRequest,NavigationService));
            

        }

        private void Execute_AddTourCommand(object obj)
        {
            NavigationService.Navigate(new AddTourView( Guide,Visibility, NavigationService));
        }
       
        private void Execute_CAncelTourCommand(object obj)
        {
            NavigationService.Navigate(new MyTours(SelectedTour,Guide, NavigationService));
        }

        private void Execute_PastToursCommand(object obj)
        {

            NavigationService.Navigate(new PastTours(SelectedTour, Guide, NavigationService));
        }

        private void Execute_BestTourCommand(object obj)
        {
            NavigationService.Navigate(new MostVisited(Guide, NavigationService));
        }


        private bool CanExecute_Command(object arg)
        {
            return true;
        }

        private void Execute_LogOutCommand(object obj)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();

            CloseAction();
        }

       

    

       



        }
}
