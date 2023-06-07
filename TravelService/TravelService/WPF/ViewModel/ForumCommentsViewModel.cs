using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class ForumCommentsViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public ForumCommentsView ForumCommentsView { get; set; }

        public ForumService _forumService;

        public CommentService _commentService;

        public Guest1Service _guestService;
        public Action CloseAction { get; set; }
        public RelayCommand BackCommand { get; set; }
        public RelayCommand ReportCommentCommand { get; set; }
        public RelayCommand AddCommentCommand { get; set; }
        public Forum SelectedForum { get; set; }
        public Comment SelectedComment { get; set; }
        public Owner Owner { get; set; }

        private ObservableCollection<Comment> _comments;
        public ObservableCollection<Comment> Comments
        {
            get => _comments;
            set
            {
                if (value != _comments)
                {
                    _comments = value;
                    OnPropertyChanged(nameof(Comments));
                }
            }
        }

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
            _guestService = new Guest1Service(Injector.CreateInstance<IGuest1Repository>());
            _commentService = new CommentService(Injector.CreateInstance<ICommentRepository>());
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
            if (_guestService.CheckCommentsOrigin(SelectedComment.User.Id))
            {
                MessageBoxResult result = MessageBox.Show($"Autor komentara nije gost.\nNe mozete ga prijaviti!", "Obavestenje", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (_guestService.CheckGuestsPresence(SelectedComment.User.Id, SelectedForum.Location))
            {
                MessageBoxResult result = MessageBox.Show($"Izabrani korisnik je bio na lokaciji {SelectedForum.Location.CityAndCountry}.\nNe mozete ga prijaviti!", "Obavestenje", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show($"Da li ste sigurni da zelite da prijavite komentar korisnika {SelectedComment.User.Username}?", "Potvrda prijave", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(result == MessageBoxResult.Yes) 
                {
                    SelectedComment.ReportsNumber = ++SelectedComment.ReportsNumber;
                    Comments = new ObservableCollection<Comment>(SelectedForum.Comments);
                    _commentService.Update(SelectedComment);
                }
            }
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
