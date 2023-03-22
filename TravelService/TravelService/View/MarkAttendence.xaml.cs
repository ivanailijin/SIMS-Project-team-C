using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    /// Interaction logic for MarkAttendence.xaml
    /// </summary>
    public partial class MarkAttendence : Window
    {
        public CheckPoint SelectedCheckPoint;
        public Tour SelectedTour;
        public Guest SelectedGuest;
        public readonly TourRepository _repositoryTour;
        public readonly GuestRepository _repositoryGuest;
        public List<Tour> _tours;
        public static ObservableCollection<Guest> Guests { get; set; }

        public MarkAttendence(Tour selectedTour, CheckPoint selectedCheckPoint,Guest selectedGuest)
        {
            InitializeComponent();
            SelectedCheckPoint = selectedCheckPoint;
            SelectedTour = selectedTour;
            SelectedGuest = selectedGuest;
            _repositoryTour = new TourRepository();
            _repositoryGuest = new GuestRepository();
            Guests = new ObservableCollection<Guest>(_repositoryGuest.GetAll());
        }
        private void Click_Cancel(object sender, RoutedEventArgs e)
        {
            SelectedGuest.Attendence = false;
            _repositoryGuest.Update(SelectedGuest);
            GuestPresence guestPresence = new GuestPresence(SelectedTour, SelectedCheckPoint);
            guestPresence.Show();
            Close();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            SelectedGuest.Attendence = true;
            _repositoryGuest.Update(SelectedGuest);
            GuestPresence guestPresence = new GuestPresence(SelectedTour, SelectedCheckPoint);
            guestPresence.Show();
            Close();
        }

    }
}
