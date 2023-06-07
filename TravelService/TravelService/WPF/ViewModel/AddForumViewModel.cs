using System;
using System.Collections.Generic;
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
    public class AddForumViewModel : ViewModelBase
    {
        private ForumService _forumService;
        private CommentService _commentService;
        private LocationService _locationService;
        private AccommodationReservationService _reservationService;
        public AddForumView AddForumView { get; set; }
        public Guest1 Guest1 { get; set; }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _location;
        public string Location
        {
            get => _location;
            set
            {
                if (value != _location)
                {
                    _location = value;
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

        public AddForumViewModel(AddForumView addForumView, Guest1 guest)
        {
            AddForumView = addForumView;
            Guest1 = guest;
            _forumService = new ForumService(Injector.CreateInstance<IForumRepository>());
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            _commentService = new CommentService(Injector.CreateInstance<ICommentRepository>());
            _reservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());

            AddForumCommand = new RelayCommand(Execute_AddForum, CanExecute_Command);
            PreviousPageCommand = new RelayCommand(Execute_PreviousPage, CanExecute_Command);
        }

        public void GoBack()
        {
            AddForumView.GoBack();
        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

        private void Execute_PreviousPage(object sender)
        {
            GoBack();
        }

        private void Execute_AddForum(object sender)
        {
            string location = Location?.Replace(",", "").Replace(" ", "");
            Location FoundLocation = _locationService.FindLocationId(location);

            bool IsMarkedComment = _reservationService.HasGuestVisitedLocation(Guest1.Id, FoundLocation.Id);

            Forum forum = new Forum(Guest1, Name, FoundLocation, DateTime.Now);
            Comment comment = new Comment(Guest1, forum, Content, DateTime.Now, IsMarkedComment);
            _forumService.Save(forum);
            _commentService.Save(comment);

            GoBack();
        }
    }
}
