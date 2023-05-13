using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class GuideHomePageViewModel : ViewModelBase
    {
        public TourRequest SelectedTourRequest { get; set; }
        private readonly GuideService _guideService;
        public Tour SelectedTour { get; set; }
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
        public Brush Background { get; set; }
        public Brush Foreground { get; set; }

        System.Windows.Media.Color lightBackgroundColor = System.Windows.Media.Color.FromRgb(255, 255, 255);
        System.Windows.Media.Color lightForegroundColor = System.Windows.Media.Color.FromRgb(0, 0, 0);
        System.Windows.Media.Color darkBackgroundColor = System.Windows.Media.Color.FromRgb(0, 0, 0);
        System.Windows.Media.Color darkForegroundColor = System.Windows.Media.Color.FromRgb(255, 255, 255);



        public GuideHomePageViewModel(Guide guide)
        {

            this.Guide = guide;
            _guideService = new GuideService(Injector.CreateInstance<IGuideRepository>());

            this.Username = guide.Username;


            LogOutCommand = new RelayCommand(Execute_LogOutCommand, CanExecute_Command);
            ActiveToursCommand = new RelayCommand(Execute_ActiveToursCommand, CanExecute_Command);
            AddTourCommand = new RelayCommand(Execute_AddTourCommand, CanExecute_Command);
            CancelTourCommand = new RelayCommand(Execute_CAncelTourCommand, CanExecute_Command);
            PastToursCommand = new RelayCommand(Execute_PastToursCommand, CanExecute_Command);
             RequestsCommand = new RelayCommand(Execute_RequestsCommand, CanExecute_Command);
            // RequestsComplexCommand = new RelayCommand(Execute_LogOutCommand, CanExecute_Command);
            BestTourCommand = new RelayCommand(Execute_BestTourCommand, CanExecute_Command);
            // AboutMeCommand = new RelayCommand(Execute_LogOutCommand, CanExecute_Command);
             ThemeCommand = new RelayCommand(Execute_ChangeThemeCommand, CanExecute_Command);
            LanguageCommand = new RelayCommand(Execute_LogOutCommand, CanExecute_Command);
        }

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


        private void Execute_RequestsCommand(object obj)
        {
            AcceptingTourRequestView accepting = new AcceptingTourRequestView(Guide, SelectedTourRequest);
            accepting.Show();

        }

        private void Execute_AddTourCommand(object obj)
        {
            AddTourView addTour = new AddTourView(Guide);
            addTour.Show();
        }
        private void Execute_ActiveToursCommand(object obj)
        {
            ActiveToursView activeTours = new ActiveToursView(SelectedTour);
            activeTours.Show();
            CloseAction();
        }
        private void Execute_CAncelTourCommand(object obj)
        {

            MyTours myTours = new MyTours(SelectedTour, Guide);
            myTours.Show();
            CloseAction();
        }

        private void Execute_PastToursCommand(object obj)
        {

            PastTours pastTours = new PastTours(SelectedTour, Guide);
            pastTours.Show();
            CloseAction();
        }

        private void Execute_BestTourCommand(object obj)
        {
            MostVisited mostVisited = new MostVisited(Guide);
            mostVisited.Show();
            CloseAction();
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

       

        private void Execute_ChangeThemeCommand(object obj)
        {
            if (this.Background is SolidColorBrush brush && brush.Color == lightBackgroundColor)
            {
                // Switch to dark theme
                this.Background = new SolidColorBrush(darkBackgroundColor);
                this.Foreground = new SolidColorBrush(darkForegroundColor);
            }
            else
            {
                // Switch to light theme
                this.Background = new SolidColorBrush(lightBackgroundColor);
                this.Foreground = new SolidColorBrush(lightForegroundColor);
            }
        }


       



        }
}
