using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for AccommodationReservationView.xaml
    /// </summary>
    public partial class AccommodationReservationView : Window, INotifyPropertyChanged
    {
        public AccommodationReservationRepository _reservationRepository;
        public Accommodation SelectedAccommodation { get; set; } 
        public ObservableCollection<AccommodationReservation> Reservations { get; set; }    
        public ObservableCollection<object> AvailableDates { get; set; }
        public AccommodationReservation SelectedAvailableDate { get; set; }

        public AccommodationReservationView(Accommodation selectedAccommodation)
        {
            InitializeComponent();
            DataContext = this;

            _reservationRepository = new AccommodationReservationRepository();
            Reservations = new ObservableCollection<AccommodationReservation>(_reservationRepository.GetAll());
            SelectedAccommodation = selectedAccommodation;

            startDatePicker.DisplayDate = DateTime.Today;
            endDatePicker.DisplayDate = DateTime.Today;
        }

        private DateTime _checkInDate;
        public DateTime CheckInDate
        {
            get => _checkInDate;
            set
            {
                if (value != _checkInDate)
                {
                    _checkInDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _checkOutDate;
        public DateTime CheckOutDate
        {
            get => _checkOutDate;
            set
            {
                if (value != _checkOutDate)
                {
                    _checkOutDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _guestNumber;
        public int GuestNumber
        {
            get => _guestNumber;
            set
            {
                if (value != _guestNumber)
                {
                    _guestNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void CheckAvailability_Click(object sender, RoutedEventArgs e)
        { 
            DateTime startDate = (DateTime)startDatePicker.SelectedDate;
            DateTime endDate = (DateTime)endDatePicker.SelectedDate;
            int daysOfStaying = int.Parse(daysOfStayingBox.Text);
            List<DateTime> reservedDates = FindReservedDates();

          /*  for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if(!reservedDates.Contains(date) && date.AddDays(3) )
            }*/
        }

        public List<DateTime> FindReservedDates()
        {
            List<DateTime> reservedDates = new List<DateTime>();

            foreach (AccommodationReservation reservation in Reservations)
            {
                if(SelectedAccommodation.Id == reservation.AccommodationId)
                {
                    DateTime checkIn = reservation.CheckInDate.Date;
                    DateTime checkOut = reservation.CheckOutDate.Date;

                    for(DateTime currentDate = checkIn; currentDate <= checkOut; currentDate.AddDays(1))
                    {
                        reservedDates.Add(currentDate);
                    }
                }
            }
            return reservedDates;
        }

        private void Reserve_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CloseReservations_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
