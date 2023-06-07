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
    public class AddCommentViewModel : ViewModelBase
    {
        private CommentService _commentService;
        private AccommodationReservationService _reservationService;
        public Guest1 Guest1 { get; set; }
        public SelectedForumViewModel SelectedForumViewModel { get; set; }
        public Action CloseAction { get; set; }

        private Forum _selectedForum;
        public Forum SelectedForum
        {
            get => _selectedForum;
            set
            {
                if (value != _selectedForum)
                {
                    _selectedForum = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _content;
        public string Content
        {
            get => _content;
            set
            {
                if (value != _content)
                {
                    _content = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _publishCommand;
        public RelayCommand PublishCommand
        {
            get => _publishCommand;
            set
            {
                if (value != _publishCommand)
                {
                    _publishCommand = value;
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

        public AddCommentViewModel(SelectedForumViewModel selectedForumViewModel, Guest1 guest, Forum selectedForum)
        {
            _commentService = new CommentService(Injector.CreateInstance<ICommentRepository>());
            _reservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
            Guest1 = guest;
            SelectedForumViewModel = selectedForumViewModel;
            SelectedForum = selectedForum;

            PublishCommand = new RelayCommand(Execute_PublishCommand, CanExecute_Command);
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

        private void Execute_PublishCommand(object sender)
        {
            bool IsMarkedComment = _reservationService.HasGuestVisitedLocation(Guest1.Id, SelectedForum.Location.Id);
            Comment comment = new Comment(Guest1, SelectedForum, Content, DateTime.Now, IsMarkedComment);
            _commentService.Save(comment);
            SelectedForumViewModel.Comments.Add(comment);
            CloseAction();
        }

        private void Execute_CancelCommand(object sender)
        {
            CloseAction();
        }
    }
}
