using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TravelService.Domain.Model;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;
using System.Linq;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for SignInForm.xaml
    /// </summary>
    public partial class SignInForm : Window
    {
        public Guide Guide { get; set; }
        private readonly UserService _userService;
        public DateTime LastOwnersLogIn { get; set; }

        private readonly GuideRepository _guideRepository;

        private readonly Guest1Service _guest1Service;

        private readonly Guest2Service _guest2Service;

        private readonly ForumService _forumService;

        private readonly OwnerService _ownerService;
        private readonly GuideService _guideService;

        private readonly AccommodationService _accommodationService;

        private readonly AccommodationReservationService _reservationService;

        private readonly GuestRepository _repositoryGuest;

        private readonly InvitationService _invitationService;

        private readonly TourRepository _tourRepository;

        private CheckPointRepository _repositoryCheckPoint;

        public List<Tour> _tours;
        public static ObservableCollection<Guest> Guests { get; set; }

        public CheckPoint SelectedCheckPoint;
        public Tour SelectedTour;
        public Guest SelectedGuest;

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SignInForm()
        {
            InitializeComponent();
            DataContext = this;
            _userService = new UserService(Injector.CreateInstance<IUserRepository>());
            _ownerService = new OwnerService(Injector.CreateInstance<IOwnerRepository>());
            _guest1Service = new Guest1Service(Injector.CreateInstance<IGuest1Repository>());
            _guideService = new GuideService(Injector.CreateInstance<IGuideRepository>());
            _reservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
            _guest2Service = new Guest2Service(Injector.CreateInstance<IGuest2Repository>());
            _accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>());
            _invitationService = new InvitationService(Injector.CreateInstance<IInvitationRepository>());
            _forumService = new ForumService(Injector.CreateInstance<IForumRepository>());
            _tourRepository = new TourRepository();
            _repositoryCheckPoint = new CheckPointRepository();
            _guideRepository = new GuideRepository();
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(txtPassword.Password))
            {
                User user = _userService.GetByUsername(Username);

                if (user != null)
                {
                    if (user.Password.Equals(txtPassword.Password))
                    {
                        if (txtPassword.Password.Equals("owner123"))
                        {
                            Owner owner = _ownerService.GetByUsername(Username);
                            OwnerWindow ownerWindow = new OwnerWindow(owner);
                            ownerWindow.Show();

                            LastOwnersLogIn = owner.LastLogIn;
                            owner.LastLogIn = DateTime.Now;
                            _ownerService.Update(owner);

                            ObservableCollection<Forum> NewForums = new ObservableCollection<Forum>();

                            foreach (Forum forum in _forumService.GetAll())
                            {
                                if (forum.DateCreated > LastOwnersLogIn)
                                {
                                    NewForums.Add(forum);
                                }
                            }

                            if (NewForums.Any())
                            {
                                MessageBoxResult result = MessageBox.Show("Na vasoj lokaciji je otvoren novi forum.\nDa li zelite da ih prikazete", "Obavestenje", MessageBoxButton.YesNo, MessageBoxImage.Information);
                                if (result == MessageBoxResult.Yes)
                                {
                                    ForumSelectionView forumSelectionView = new ForumSelectionView(owner);
                                    ownerWindow?.SwitchToPage(forumSelectionView);
                                }
                            }

                            Close();
                        }
                        else if (txtPassword.Password.Equals("guest1123"))
                        {
                            Guest1 guest1 = _guest1Service.GetByUsername(Username);
                            FirstGuestWindow firstGuestWindow = new FirstGuestWindow(guest1);
                            firstGuestWindow.Show();
                            Close();
                        }
                        else if (txtPassword.Password.Equals("guest2123"))
                        {
                            Guest2 guest2 = _guest2Service.GetByUsername(Username);
                            foreach (Invitation invitation in _invitationService.GetAll())
                            {
                                if (invitation.GuestId == guest2.Id && invitation.GuestAttendence == false)
                                {
                                    MarkAttendence markAttendence = new MarkAttendence(SelectedTour, SelectedCheckPoint, guest2);
                                    markAttendence.ShowDialog();
                                    Close();
                                }
                            }
                            SecondGuestView secondGuestView = new SecondGuestView(guest2);
                            secondGuestView.Show();
                            Close();
                        }
                        else if (txtPassword.Password.Equals("guide123"))
                        {
                            Guide Guide = _guideService.GetByUsername(Username);
                            GuideHomePageView view = new GuideHomePageView(Guide);
                            view.Show();

                            List<Tour> TourList = _tourRepository.GetAll();

                            /*foreach (Tour tour in TourList)
                            {
                                Tour thisTour = _tourRepository.FindById(tour.Id);

                                if (thisTour.GuideId == guide.Id)
                                {
                                    MessageBoxResult result = MessageBox.Show("Do you want to see your future Tours?", "Notification", MessageBoxButton.YesNo);
                                    if (result == MessageBoxResult.Yes)
                                    {
                                        MyTours myTours = new MyTours(SelectedTour, guide);
                                        myTours.Show();
                                        Close();
                                    }
                                    break;
                                }
                            }*/

                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Wrong user type!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Wrong password!");
                    }
                }
                else
                {
                    MessageBox.Show("Wrong username!");
                }
            }
        }

    }
}


