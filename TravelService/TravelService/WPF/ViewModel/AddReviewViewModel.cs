using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class AddReviewViewModel : ViewModelBase
    {
        private readonly TourReviewService _tourReviewService;
        public Tour SelectedTour { get; set; }
        public Guest2 Guest2 { get; set; }
        public Action CloseAction { get; set; }
        public IEnumerable<int> Grades => Enumerable.Range(1, 5);

        private RelayCommand _rateCommand;
        public RelayCommand RateCommand
        {
            get => _rateCommand;
            set
            {
                if (value != _rateCommand)
                {
                    _rateCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        private RelayCommand _voucherViewCommand;
        public RelayCommand VoucherViewCommand
        {
            get => _voucherViewCommand;
            set
            {
                if (value != _voucherViewCommand)
                {
                    _voucherViewCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _addPictureCommand;
        public RelayCommand AddPictureCommand
        {
            get => _addPictureCommand;
            set
            {
                if (value != _addPictureCommand)
                {
                    _addPictureCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _cancelCommand;
        public RelayCommand CancelCommand
        {
            get => _cancelCommand;
            set
            {
                if (value != _cancelCommand)
                {
                    _cancelCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        private RelayCommand _homePageCommand;
        public RelayCommand HomePageCommand
        {
            get => _homePageCommand;
            set
            {
                if (value != _homePageCommand)
                {
                    _homePageCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _tourEntertainment;
        public int TourEntertainment
        {
            get => _tourEntertainment;
            set
            {
                if (value != _tourEntertainment)
                {
                    _tourEntertainment = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _guideLanguage;
        public int GuideLanguage
        {
            get => _guideLanguage;
            set
            {
                if (value != _guideLanguage)
                {
                    _guideLanguage = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _guideKonwledge;
        public int GuideKnowledge
        {
            get => _guideKonwledge;
            set
            {
                if (value != _guideKonwledge)
                {
                    _guideKonwledge = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _comment;
        public string Comment
        {
            get => _comment;
            set
            {
                if (value != _comment)
                {
                    _comment = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _valid;
        public bool Valid
        {
            get => _valid;
            set
            {
                if (value != _valid)
                {
                    _valid = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _pictures;
        public string Pictures
        {
            get => _pictures;
            set
            {
                if (value != _pictures)
                {
                    _pictures = value;
                    OnPropertyChanged();
                }
            }
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
        public AddReviewViewModel(Tour selectedTour, Guest2 guest2) 
        {
           
            _tourReviewService = new TourReviewService(Injector.CreateInstance<ITourReviewRepository>());
            SelectedTour = selectedTour;
            Guest2 = guest2;
            Username = guest2.Username;
            RateCommand = new RelayCommand(Execute_Rate, CanExecute_Command);
            HomePageCommand = new RelayCommand(Execute_HomePageCommand, CanExecute_Command);
            AddPictureCommand = new RelayCommand(Execute_AddPicture, CanExecute_Command);
            CancelCommand = new RelayCommand(Execute_Cancel, CanExecute_Command);
            VoucherViewCommand = new RelayCommand(Execute_VoucherViewCommand, CanExecute_Command);
        }
        private bool CanExecute_Command(object parameter)
        {
            return true;
        }
        private void Execute_Rate(object sender)
        {
            _tourReviewService.addReview(GuideKnowledge,GuideLanguage,TourEntertainment,Comment,Pictures,SelectedTour, Guest2);
            CloseAction();
        }
        private void Execute_HomePageCommand(object sender)
        {
            SecondGuestView secondGuestView = new SecondGuestView(Guest2);
            secondGuestView.Show();
            CloseAction();
        }
        private void Execute_VoucherViewCommand(object sender)
        {
            GuestsVouchersView guestsVouchersView = new GuestsVouchersView(Guest2);
            guestsVouchersView.Show();
        }
        private void Execute_AddPicture(object sender)
        {
            string newPictures = _tourReviewService.addPictures(Pictures);
            Pictures = _tourReviewService.addToPictureList(newPictures, Pictures);
        }
        private void Execute_Cancel(object sender)
        {
            CloseAction();
        }
    }
}