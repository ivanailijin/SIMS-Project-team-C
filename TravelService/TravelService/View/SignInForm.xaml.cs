using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TravelService.Model;
using TravelService.Repository;

namespace TravelService.View
{
    /// <summary>
    /// Interaction logic for SignInForm.xaml
    /// </summary>
    public partial class SignInForm : Window
    { 
        private readonly UserRepository _repository;

        private readonly Guest1Repository _guest1Repository;

        private readonly OwnerRepository _ownerRepository;

        private readonly AccommodationRepository _accommodationRepository;

        private readonly AccommodationReservationRepository _reservationRepository;
        public readonly TourRepository _repositoryTour;
        public readonly GuestRepository _repositoryGuest;
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
            _repository = new UserRepository();
            _ownerRepository = new OwnerRepository();
            _guest1Repository = new Guest1Repository();
            _reservationRepository = new AccommodationReservationRepository();
            _accommodationRepository = new AccommodationRepository();
            _repositoryTour = new TourRepository();
            _repositoryGuest = new GuestRepository();
            _repositoryCheckPoint = new CheckPointRepository();


        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(txtPassword.Password))
            {
                User user = _repository.GetByUsername(Username);

                if (user != null)
                {

                    if (user.Password.Equals(txtPassword.Password))
                    {
                        if (txtPassword.Password.Equals("owner123"))
                        {
                            Owner owner = _ownerRepository.GetByUsername(Username);
                            OwnerView ownerView = new OwnerView(owner);
                            ownerView.Show();

                            List<AccommodationReservation> reservationList = _reservationRepository.GetAll();

                            foreach (AccommodationReservation reservation in reservationList)
                            {
                                Accommodation reservedAccommodation = _accommodationRepository.FindById(reservation.AccommodationId);
                                TimeSpan dayDifference = DateTime.Today - reservation.CheckOutDate;
                                if (!reservation.IsRated && dayDifference.Days < 5 && dayDifference.Days > 0 && reservedAccommodation.OwnerId == owner.Id)
                                {
                                    MessageBoxResult result = MessageBox.Show("You have an unrated guest?\nDo you want to rate it now?", "Notification", MessageBoxButton.YesNo);
                                    if (result == MessageBoxResult.Yes)
                                    {
                                        GuestRatingOverview guestRatingOverview = new GuestRatingOverview(owner);
                                        guestRatingOverview.ShowDialog();
                                    }
                                    break;
                                }
                            }
                            Close();
                        }
                        else if (txtPassword.Password.Equals("guest1123"))
                        {
                            Guest1 guest1 = _guest1Repository.GetByUsername(Username);
                            AccommodationView accommodationView = new AccommodationView(guest1);
                            accommodationView.Show();
                            Close();
                        }
                        else if (txtPassword.Password.Equals("guest2123"))
                        {

                           MarkAttendence markAttendence = new MarkAttendence(SelectedTour, SelectedCheckPoint, SelectedGuest);
                           markAttendence.ShowDialog();
                           Close();
                            
                           SecondGuestView secondGuestView = new SecondGuestView();
                           secondGuestView.Show();
                           Close();
                        }
                        else if (txtPassword.Password.Equals("guide123"))
                        {
                            Window1 window1 = new Window1();
                            window1.Show();
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


