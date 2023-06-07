using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.WPF.ViewModel
{
    public class ClosingForumViewModel : ViewModelBase
    {
        private ForumService _forumService;
        public Guest1 Guest1 { get; set; }
        public Action CloseAction { get; set; }
        public SelectedForumViewModel SelectedForumViewModel { get; set; }

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

        private RelayCommand confirmCommand;
        public RelayCommand ConfirmCommand
        {
            get => confirmCommand;
            set
            {
                if (value != confirmCommand)
                {
                    confirmCommand = value;
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

        public ClosingForumViewModel(Forum selectedForum, SelectedForumViewModel selectedForumViewModel)
        {
            _forumService = new ForumService(Injector.CreateInstance<IForumRepository>());
            SelectedForum = selectedForum;
            SelectedForumViewModel = selectedForumViewModel;

            ConfirmCommand = new RelayCommand(Execute_ConfirmCommand, CanExecute_Command);
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

        private void Execute_ConfirmCommand(object sender)
        {
            Forum forum = _forumService.CloseForum(SelectedForum.Id);
            SelectedForumViewModel.SelectedForum = forum;
            CloseAction();
        }

        private void Execute_CancelCommand(object sender)
        {
            CloseAction();
        }
    }
}
