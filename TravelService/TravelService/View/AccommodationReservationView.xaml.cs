using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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
        public Guest1 LoggedInGuest1 { get; set; }
        public ObservableCollection<AccommodationReservation> Reservations { get; set; }
        public ObservableCollection<Tuple<DateTime, DateTime>> AvailableDatesPair { get; set; }
        public Tuple<DateTime, DateTime> SelectedAvailableDatePair { get; set; }

        public AccommodationReservationView(Accommodation selectedAccommodation, Guest1 guest1)
        {
            InitializeComponent();
            DataContext = this;

            _reservationRepository = new AccommodationReservationRepository();
            Reservations = new ObservableCollection<AccommodationReservation>(_reservationRepository.GetAll());
            SelectedAccommodation = selectedAccommodation;
            this.LoggedInGuest1 = guest1;

            startDatePicker.DisplayDateStart = DateTime.Today;
            endDatePicker.DisplayDateStart = DateTime.Today;
            AvailableDatesPair = new ObservableCollection<Tuple<DateTime, DateTime>>();
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

        private int _lengthOfStay;
        public int LengthOfStay
        {
            get => _lengthOfStay;
            set
            {
                if (value != _lengthOfStay)
                {
                    _lengthOfStay = value;
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

        private bool _isRated;
        public bool IsRated
        {
            get => _isRated;
            set
            {
                if (value != _isRated)
                {
                    _isRated = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Error
        {
            get
            {
                return string.Empty;
            }
        }

        public string this[string columnName] => throw new NotImplementedException();

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
            List<Tuple<DateTime, DateTime>> availableDateRange = new List<Tuple<DateTime, DateTime>>();
            

            if (endDate < startDate)
            {
                if (AvailableDatesPair != null)
                {
                    AvailableDatesPair.Clear();
                }
                MessageBox.Show("End date must be greater than start date. Please try again.");
                return;
            }

            if(daysOfStaying < SelectedAccommodation.MinReservationDays)
            {
                if (AvailableDatesPair != null)
                {
                    AvailableDatesPair.Clear();
                }
                MessageBox.Show($"Minimum number of days for reservation is {SelectedAccommodation.MinReservationDays}");
                return;
            }

            

            //_reservationRepository.FindAvailableDates(SelectedAccommodation, AvailableDatesPair, startDate, endDate, daysOfStaying, NotificationBlock.Text);

            List<DateTime> reservedDates = FindReservedDates();
            List<DateTime> availableDates = new List<DateTime>();
            

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (!reservedDates.Contains(date))
                {
                    availableDates.Add(date);
                }
                else
                {
                    availableDates.Clear();
                }

                if (availableDates.Count == daysOfStaying)
                {
                    AvailableDatesPair.Add(Tuple.Create(availableDates[0].Date, availableDates[availableDates.Count - 1].Date));
                    availableDates.RemoveAt(0);
                }
            }

            availableDates.Clear();
            DateTime recommendedStartDate = startDate;
            DateTime recommendedEndDate = endDate;

            if (AvailableDatesPair.Count == 0)
            {
                NotificationBlock.Text = "All dates in the given range are taken. We recommend the following dates: ";

                while (!(AvailableDatesPair.Count >= 5))
                {
                    recommendedStartDate = recommendedStartDate.Equals(DateTime.Today) ? recommendedStartDate : recommendedStartDate.AddDays(-1);
                    recommendedEndDate = recommendedEndDate.AddDays(1);

                    availableDates.Clear();
                    for (DateTime date = recommendedStartDate; date <= recommendedEndDate; date = date.AddDays(1))
                    {
                        if (!reservedDates.Contains(date))
                        {
                            availableDates.Add(date);
                        }
                        else
                        {
                            availableDates.Clear();
                        }

                        if (availableDates.Count == daysOfStaying)
                        {
                             if (!AvailableDatesPair.Contains(Tuple.Create(availableDates[0].Date, availableDates[availableDates.Count - 1].Date)))
                                AvailableDatesPair.Add(Tuple.Create(availableDates[0].Date, availableDates[availableDates.Count - 1].Date));
                             availableDates.RemoveAt(0);
                        } 
                    }
                }
            }
            
        }

       public List<DateTime> FindReservedDates()
        {
            List<DateTime> reservedDates = new List<DateTime>();

            foreach (AccommodationReservation reservation in Reservations)
            {
                if (SelectedAccommodation.Id == reservation.AccommodationId)
                {
                    DateTime checkIn = reservation.CheckInDate;
                    DateTime checkOut = reservation.CheckOutDate;

                    for (DateTime currentDate = checkIn; currentDate <= checkOut; currentDate = currentDate.AddDays(1))
                    {
                            reservedDates.Add(currentDate);
                    }
                }
            }
            return reservedDates;
        }

        private void Reserve_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedAvailableDatePair != null)
            {
                if(int.Parse(guestNumberBox.Text) > SelectedAccommodation.MaxGuestNumber)
                {
                    MessageBox.Show($"Maximum number of guests for {SelectedAccommodation.Name} accommodation is {SelectedAccommodation.MaxGuestNumber}");
                    return;
                }

                CheckInDate = SelectedAvailableDatePair.Item1;
                CheckOutDate = SelectedAvailableDatePair.Item2;
                IsRated = false;
                AccommodationReservation reservation = new AccommodationReservation(SelectedAccommodation.Id, SelectedAccommodation.Name, LoggedInGuest1.Id, CheckInDate, CheckOutDate, LengthOfStay, GuestNumber, IsRated);
                _reservationRepository.Save(reservation);
                Close();
            }
            else
            {
                MessageBox.Show("Please choose date range for your reservation.");
            }
        }

        private void CloseReservations_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        
    }
}
