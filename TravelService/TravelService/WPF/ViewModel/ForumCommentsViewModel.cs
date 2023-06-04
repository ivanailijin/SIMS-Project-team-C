using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class ForumCommentsViewModel : ViewModelBase
    {
        public ForumCommentsView ForumCommentsView { get; set; }

        public ForumService _forumService;
        public Action CloseAction { get; set; }
        public RelayCommand BackCommand { get; set; }
        public RelayCommand ReportCommentCommand { get; set; }
        public RelayCommand AddCommentCommand { get; set; }
        public Forum SelectedForum { get; set; }
        public ObservableCollection<Comment> Comments { get; set; }
        public Owner Owner { get; set; }


        private int _numberOwnerComments;
        public int NumberOwnersComments
        {
            get => _numberOwnerComments;
            set
            {
                if (value != _numberOwnerComments)
                {
                    _numberOwnerComments = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _numberGuestComments;
        public int NumberGuestsComments
        {
            get => _numberGuestComments;
            set
            {
                if (value != _numberGuestComments)
                {
                    _numberGuestComments = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _ownerAuthorized;
        public bool OwnerAuthorized
        {
            get => _ownerAuthorized;
            set
            {
                if (value != _ownerAuthorized)
                {
                    _ownerAuthorized = value;
                    OnPropertyChanged();
                }
            }
        }

        public ForumCommentsViewModel(Owner owner, ForumCommentsView forumCommentsView, Forum selectedForum)
        {
            InitializeCommands();
            _forumService = new ForumService(Injector.CreateInstance<IForumRepository>());
            SelectedForum = selectedForum;
            Comments = new ObservableCollection<Comment>(SelectedForum.Comments);
            this.Owner = owner;
            ForumCommentsView = forumCommentsView;
            NumberGuestsComments = _forumService.GetNumberOfGuestComments(SelectedForum);
            NumberOwnersComments = _forumService.GetNumberOfOwnerComments(SelectedForum);
            OwnerAuthorized = _forumService.GetOwnersAuthorization(Owner, SelectedForum);
        }
        private void InitializeCommands()
        {
            BackCommand = new RelayCommand(Execute_BackCommand, CanExecute_Command);
            ReportCommentCommand = new RelayCommand(Execute_ReportCommentCommand, CanExecute_Command);
            AddCommentCommand = new RelayCommand(Execute_AddCommentCommand, CanExecute_Command);
        }
        private void Execute_ReportCommentCommand(object obj)
        {

        }
        private void Execute_ShowForumCommand(object obj)
        {

        }
        private void Execute_AddCommentCommand(object obj)
        {
            AddingForumComment addingForumComment = new AddingForumComment(Owner, SelectedForum, Comments);
            addingForumComment.Show();
        }
        private void Execute_BackCommand(object obj)
        {
            ForumCommentsView.GoBack();
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }
    }
}
