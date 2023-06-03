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
        public ObservableCollection<Forum> Forums { get; set; }
        public Owner Owner { get; set; }

        public ForumCommentsViewModel(Owner owner, ForumCommentsView forumCommentsView, Forum selectedForum)
        {
            InitializeCommands();
            _forumService = new ForumService(Injector.CreateInstance<IForumRepository>());
            Forums = new ObservableCollection<Forum>(_forumService.GetAll());
            SelectedForum = selectedForum; 
            this.Owner = owner;
            ForumCommentsView = forumCommentsView;
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
