﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TravelService.Domain.Model;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for SignInForm.xaml
    /// </summary>
    public partial class SignInForm : Window
    {
        private readonly UserService _userService;

        private readonly GuideRepository _guideRepository;

        private readonly Guest1Service _guest1Service;

        private readonly Guest2Service _guest2Service;

        private readonly OwnerService _ownerService;

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
            _reservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
            _guest2Service = new Guest2Service(Injector.CreateInstance<IGuest2Repository>());
            _accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>());
            _invitationService = new InvitationService(Injector.CreateInstance<IInvitationRepository>());
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
                            OwnerView ownerView = new OwnerView(owner);
                            ownerView.Show();

                            List<AccommodationReservation> reservationList = _reservationService.GetAll();

                            foreach (AccommodationReservation reservation in reservationList)
                            {
                                Accommodation reservedAccommodation = _accommodationService.FindById(reservation.AccommodationId);
                                TimeSpan dayDifference = DateTime.Today - reservation.CheckOutDate;
                                if (!reservation.IsRated && dayDifference.Days < 5 && dayDifference.Days > 0 && reservedAccommodation.OwnerId == owner.Id)
                                {
                                    MessageBoxResult result = MessageBox.Show("Imate neocenjene goste.\nDa li zelite da ih ocenite odmah?", "Obavestenje", MessageBoxButton.YesNo, MessageBoxImage.Information);
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
                            Guest1 guest1 = _guest1Service.GetByUsername(Username);
                            FirstGuestView firstGuestView = new FirstGuestView(guest1);
                            firstGuestView.Show();
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
                            Guide guide = _guideRepository.GetByUsername(Username);
                            GuideHomePageView guideHomePage = new GuideHomePageView(guide);
                            guideHomePage.Show();

                            List<Tour> TourList = _tourRepository.GetAll();

                            foreach (Tour tour in TourList)
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
                            }

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


