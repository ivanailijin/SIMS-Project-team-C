using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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

        private readonly AccommodationReservationRepository _reservationRepository;

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
            _reservationRepository = new AccommodationReservationRepository();
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            User user = _repository.GetByUsername(Username);
            if (user != null)
            {
                if (user.Username.Equals("Owner") &&  user.Password == txtPassword.Password)
                {
                    OwnerView ownerView = new OwnerView();
                    ownerView.Show();

                    List<AccommodationReservation> reservationList = _reservationRepository.GetAll();

                    foreach(AccommodationReservation reservation in reservationList)
                    {
                        TimeSpan dayDifference = DateTime.Today - reservation.CheckOutDate;
                        if(!reservation.IsRated && dayDifference.Days < 5 && dayDifference.Days > 0)
                        {
                            MessageBoxResult result = MessageBox.Show("You have an unrated guest?\nDo you want to rate it now?", "Rating guest", MessageBoxButton.YesNo);
                            if(result == MessageBoxResult.Yes)
                            {
                                GuestRatingView guestRatingView = new GuestRatingView(reservation.Id);
                                guestRatingView.Show();
                            }
                        }
                    }
                    
                    Close();
                }
                else if(user.Username.Equals("Guest1") && user.Password == txtPassword.Password)
                {
                    AccommodationView accommodationView = new AccommodationView();
                    accommodationView.Show();
                    Close();
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
