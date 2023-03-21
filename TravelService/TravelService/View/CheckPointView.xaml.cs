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
    /// Interaction logic for CheckPointView.xaml
    /// </summary>
    public partial class CheckPointView : Window
    {

        public Tour SelectedTour { get; set; }
        public CheckPoint SelectedCheckPoint { get; set; }
        private ObservableCollection<CheckPoint> _checkPoints;
        public static ObservableCollection<Tour> Tours { get; set; }
        public static List<CheckPoint> CheckPoints { get; set; }
        public static List<CheckPoint> FilteredCheckPoint { get; set; } 
        public readonly CheckPointRepository _repositoryCheckPoint;

        public readonly TourRepository _tourRepository;

        
        public CheckPointView(Tour selectedTour)
        {
            InitializeComponent();
            DataContext = this;
            _tourRepository = new TourRepository();
            _repositoryCheckPoint = new CheckPointRepository();

            Tours = new ObservableCollection<Tour>(_tourRepository.GetAll());
            CheckPoints = new List<CheckPoint>(_repositoryCheckPoint.GetAll());
            SelectedTour = selectedTour;

            FilteredCheckPoint = new List<CheckPoint>();


           // FilteredCheckPoint.ElementAt(0).Active = true;
          //  SelectedCheckPoint = FilteredCheckPoint.ElementAt(0);
            foreach (Tour tour in Tours)
            {

                List<CheckPoint> ListCheckPoints = new List<CheckPoint>();

                tour.CheckPoints.Clear();
                ListCheckPoints.Clear();

                int currentId = tour.Id;


                foreach (CheckPoint c in CheckPoints)
                {
                    int currentCheckPointTourId = c.TourId;
                    if (currentCheckPointTourId == currentId)
                    {
                        ListCheckPoints.Add(c);

                        if (SelectedTour.Id == currentCheckPointTourId)
                        {
                            FilteredCheckPoint.Add(c);
                        }
                    }
                }

                tour.CheckPoints.AddRange(ListCheckPoints);

                _checkPoints = new ObservableCollection<CheckPoint>(_repositoryCheckPoint.GetAll());
                _checkPoints.ElementAt(0).Active = true;
                SelectedCheckPoint = _checkPoints.ElementAt(0);

                ListCheckBox.ItemsSource = FilteredCheckPoint;
                if (FilteredCheckPoint.Count > 0)
                {
                    FilteredCheckPoint[0].Active = true;
                }

            }
        }


    
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


       
        private void Mark_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)this.FindName("myCheckBox");
            if (SelectedCheckPoint != null)
            {
                
             //   if (checkBox.IsChecked == true)
               

                    GuestPresence guestPresence = new GuestPresence(SelectedTour,SelectedCheckPoint);
                    guestPresence.Show();
                    Close();
                

            }
        }
        private int numChecked = 0;

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

            // Increment the number of checked checkboxes
            numChecked++;
             ListCheckBox.ItemsSource = FilteredCheckPoint;
            // Check if all checkboxes are checked
            if (numChecked + 1 == ListCheckBox.Items.Count)
            {
             
                
                    // Enable or disable the button based on the value of Active property
                    EndButton.IsEnabled = true;
                
               
              
            }
        }
        private void End_Click(object sender, RoutedEventArgs e)
        {
            

            SelectedTour.Done = true;
            _tourRepository.Update(SelectedTour);
            MessageBox.Show("The tour was successfully completed");
            TourOverview tourOverview = new TourOverview(SelectedTour);
            tourOverview.Show();
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            
            MessageBox.Show("The tour is cancelled");
            TourOverview tourOverview = new TourOverview(SelectedTour);
            tourOverview.Show();
            Close();

        }
    }
}
      