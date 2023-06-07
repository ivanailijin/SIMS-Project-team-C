using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class ForumsViewModel : ViewModelBase
    {
        private ForumService _forumService;
        public ForumsView ForumView { get; set; }
        public Guest1 Guest1 { get; set; }
        public Forum SelectedForum { get; set; }

        private ObservableCollection<Forum> _forums;
        public ObservableCollection<Forum> Forums
        {
            get { return _forums; }
            set
            {
                _forums = value;
                OnPropertyChanged(nameof(_forums));
            }
        }

        private ObservableCollection<Forum> _myForums;
        public ObservableCollection<Forum> MyForums
        {
            get { return _myForums; }
            set
            {
                _myForums = value;
                OnPropertyChanged(nameof(_myForums));
            }
        }

        private RelayCommand _forumSelectedCommand;
        public RelayCommand ForumSelectedCommand
        {
            get => _forumSelectedCommand;
            set
            {
                if (value != _forumSelectedCommand)
                {
                    _forumSelectedCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _addForumCommand;
        public RelayCommand AddForumCommand
        {
            get => _addForumCommand;
            set
            {
                if (value != _addForumCommand)
                {
                    _addForumCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        public ForumsViewModel(ForumsView forumsView, Guest1 guest)
        {
            ForumView = forumsView;
            Guest1 = guest;
            _forumService = new ForumService(Injector.CreateInstance<IForumRepository>());
            Forums = new ObservableCollection<Forum>(_forumService.GetAll());
            MyForums = new ObservableCollection<Forum>(_forumService.FindByGuestId(Guest1.Id));

            ForumSelectedCommand = new RelayCommand(Execute_OnItemSelected, CanExecute_Command);
            AddForumCommand = new RelayCommand(Execute_AddForum, CanExecute_Command);
        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

        private void Execute_OnItemSelected(object sender)
        {
            SelectedForumView selectedForumView = new SelectedForumView(Guest1, SelectedForum, this);
            FirstGuestWindow firstGuestWindow = Window.GetWindow(ForumView) as FirstGuestWindow ?? new(Guest1);
            firstGuestWindow?.SwitchToPage(selectedForumView);
        }

        private void Execute_AddForum(object sender)
        {
            AddForumView addForumView = new AddForumView(Guest1);
            FirstGuestWindow firstGuestWindow = Window.GetWindow(ForumView) as FirstGuestWindow ?? new(Guest1);
            firstGuestWindow?.SwitchToPage(addForumView);
        }
    }
}
