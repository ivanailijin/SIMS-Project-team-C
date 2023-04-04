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
        public Guest2 SelectedGuest;
        public readonly TourRepository _repositoryTour;
        public readonly Guest2Repository _repositoryGuest;
        public List<Tour> _tours;
        public static ObservableCollection<Guest2> Guests { get; set; }

        public Guest2 Guest2 { get; set; }

        public MarkAttendence(Tour selectedTour, CheckPoint selectedCheckPoint,Guest2 guest2)
        {
            InitializeComponent();
            SelectedCheckPoint = selectedCheckPoint;
            SelectedTour = selectedTour;
            this.Guest2 = guest2;
            _repositoryTour = new TourRepository();
            _repositoryGuest = new Guest2Repository();
            Guests = new ObservableCollection<Guest2>(_repositoryGuest.GetAll());
        }
        private void Click_Cancel(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Thank you for answering!");
            SecondGuestView secondGuestView = new SecondGuestView(Guest2);
            secondGuestView.Show();
            Close();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            
            MessageBox.Show("Thank you for confirming!");
            SecondGuestView secondGuestView = new SecondGuestView(Guest2);
            secondGuestView.Show();
            Close();
        }

    }
}
