using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.Services;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class SelectedForumViewModel : ViewModelBase
    {
        private ForumService _forumService;
        private CommentService _commentService;
        public Guest1 Guest1 { get; set; }
        public SelectedForumView SelectedForumView { get; set; }
        public ForumsViewModel ForumsViewModel { get; set; }

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

        private bool _isForumOwner;
        public bool IsForumOwner
        {
            get { return _isForumOwner; }
            set
            {
                _isForumOwner = value;
                OnPropertyChanged();
            }
        }

        private bool _isOwnerAccommodationOnLocation;
        public bool IsOwnerAccommodationOnLocation
        {
            get { return _isOwnerAccommodationOnLocation; }
            set
            {
                _isOwnerAccommodationOnLocation = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Comment> _comments;
        public ObservableCollection<Comment> Comments
        {
            get => _comments;
            set
            {
                if (value != _comments)
                {
                    _comments = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _previousPageCommand;
        public RelayCommand PreviousPageCommand
        {
            get => _previousPageCommand;
            set
            {
                if (value != _previousPageCommand)
                {
                    _previousPageCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _addCommentCommand;
        public RelayCommand AddCommentCommand
        {
            get => _addCommentCommand;
            set
            {
                if (value != _addCommentCommand)
                {
                    _addCommentCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _closeForumCommand;
        public RelayCommand CloseForumCommand
        {
            get => _closeForumCommand;
            set
            {
                if (value != _closeForumCommand)
                {
                    _closeForumCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        public SelectedForumViewModel(SelectedForumView selectedForumView, Guest1 guest, Forum selectedForum, ForumsViewModel forumsViewModel)
        {
            _forumService = new ForumService(Injector.CreateInstance<IForumRepository>());
            _commentService = new CommentService(Injector.CreateInstance<ICommentRepository>());
            Guest1 = guest;
            SelectedForumView = selectedForumView;
            SelectedForum = selectedForum;
            ForumsViewModel = forumsViewModel;
            Comments = new ObservableCollection<Comment>(_commentService.FindByForumId(SelectedForum.Id));
            IsForumOwner = _forumService.IsUserForumOwner(Guest1.Id, SelectedForum);

            PreviousPageCommand = new RelayCommand(Execute_PreviousPage, CanExecute_Command);
            AddCommentCommand = new RelayCommand(Execute_AddComment, CanExecute_Command);
            CloseForumCommand = new RelayCommand(Execute_CloseForum, CanExecute_Command);
        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

        public void GoBack()
        {
            SelectedForumView.GoBack();
        }

        private void Execute_PreviousPage(object sender)
        {
            ForumsViewModel.SelectedForum = SelectedForum;
            GoBack();
        }
        private void Execute_AddComment(object sender)
        {
            AddCommentView addCommentView = new AddCommentView(this, Guest1, SelectedForum);
            addCommentView.Show();
        }

        private void Execute_CloseForum(object sender)
        {
            ClosingForumView closingForumView = new ClosingForumView(SelectedForum, this);
            closingForumView.Show();
        }
    }
}
