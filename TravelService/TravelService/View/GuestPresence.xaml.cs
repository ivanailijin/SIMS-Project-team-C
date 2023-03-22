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
    /// Interaction logic for GuestPresence.xaml
    /// </summary>
    public partial class GuestPresence : Window
    {
        public Guest SelectedGuest { get; set; }
        private GuestRepository _repositoryGuest;
        private ObservableCollection<Guest> _guests;
        private CheckPointRepository _repositoryCheckPoint;
        public CheckPoint SelectedCheckPoint { get; set; }  
        public Tour SelectedTour { get; set; }  

        public GuestPresence(Tour selectedTour, CheckPoint selectedcCheckPoint)
        { 
            InitializeComponent();
            DataContext = this;
            _repositoryGuest= new GuestRepository();
            _repositoryCheckPoint = new CheckPointRepository();
            SelectedTour = selectedTour;
            SelectedCheckPoint = selectedcCheckPoint;
            _guests = new ObservableCollection<Guest>(filterGuests(_repositoryGuest.GetAll()));
            GuestDataGrid.ItemsSource = _guests;
        }

        public List<Guest> filterGuests(List<Guest> guests)
        {
            List<Guest> filteredGuests = new List<Guest>();
            foreach (Guest guest in guests)
            {
                if (guest.TourId == SelectedTour.Id)
                {
                    filteredGuests.Add(guest);
                }
            }
            return filteredGuests;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Mark_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedGuest != null)
            {
                MarkAttendence markAttendence= new MarkAttendence(SelectedTour,SelectedCheckPoint,SelectedGuest);
                markAttendence.Show();
                Close();
            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            CheckPointView checkPointView = new CheckPointView(SelectedTour);
            checkPointView.Show();
        }
    }
}
