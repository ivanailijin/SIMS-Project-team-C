using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
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

        private RelayCommand rateCommand;
        public RelayCommand RateCommand
        {
            get => rateCommand;
            set
            {
                if (value != rateCommand)
                {
                    rateCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand addPictureCommand;
        public RelayCommand AddPictureCommand
        {
            get => addPictureCommand;
            set
            {
                if (value != addPictureCommand)
                {
                    addPictureCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand cancelCommand;
        public RelayCommand CancelCommand
        {
            get => cancelCommand;
            set
            {
                if (value != cancelCommand)
                {
                    cancelCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private int tourEntertainment;
        public int TourEntertainment
        {
            get => tourEntertainment;
            set
            {
                if (value != tourEntertainment)
                {
                    tourEntertainment = value;
                    OnPropertyChanged();
                }
            }
        }

        private int guideLanguage;
        public int GuideLanguage
        {
            get => guideLanguage;
            set
            {
                if (value != guideLanguage)
                {
                    guideLanguage = value;
                    OnPropertyChanged();
                }
            }
        }

        private int guideKonwledge;
        public int GuideKnowledge
        {
            get => guideKonwledge;
            set
            {
                if (value != guideKonwledge)
                {
                    guideKonwledge = value;
                    OnPropertyChanged();
                }
            }
        }

        private string comment;
        public string Comment
        {
            get => comment;
            set
            {
                if (value != comment)
                {
                    comment = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool valid;
        public bool Valid
        {
            get => valid;
            set
            {
                if (value != valid)
                {
                    valid = value;
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
        public AddReviewViewModel(Tour selectedTour, Guest2 guest2) 
        {
            _tourReviewService = new TourReviewService(Injector.CreateInstance<ITourReviewRepository>());
            SelectedTour = selectedTour;
            Guest2 = guest2;

            RateCommand = new RelayCommand(Execute_Rate, CanExecute_Command);
            AddPictureCommand = new RelayCommand(Execute_AddPicture, CanExecute_Command);
            CancelCommand = new RelayCommand(Execute_Cancel, CanExecute_Command);
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