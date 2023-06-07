using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class ForumSelectionViewModel : ViewModelBase
    {
        public ForumSelectionView ForumSelectionView { get; set; }

        public ForumService _forumService;
        public Action CloseAction { get; set; }
        public RelayCommand BackCommand { get; set; }
        public RelayCommand ShowForumCommand { get; set; }

        public Forum SelectedForum { get; set; }
        public ObservableCollection<Forum> Forums { get; set; }
        public Owner Owner { get; set; }

        public ForumSelectionViewModel(Owner owner, ForumSelectionView forumSelectionView)
        {
            InitializeCommands();
            _forumService = new ForumService(Injector.CreateInstance<IForumRepository>());
            Forums = new ObservableCollection<Forum>(_forumService.GetAll());
            this.Owner = owner;
            ForumSelectionView = forumSelectionView;
        }
        private void InitializeCommands()
        {
            BackCommand = new RelayCommand(Execute_BackCommand, CanExecute_Command);
            ShowForumCommand = new RelayCommand(Execute_ShowForumCommand, CanExecute_Command);
        }
        private void Execute_ShowForumCommand(object obj)
        {
            ForumCommentsView forumCommentsView = new ForumCommentsView(Owner, SelectedForum);
            OwnerWindow ownerWindow = Window.GetWindow(ForumSelectionView) as OwnerWindow;
            ownerWindow?.SwitchToPage(forumCommentsView);
        }
        private void Execute_BackCommand(object obj)
        {
            ForumSelectionView.GoBack();
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }
    }
}
