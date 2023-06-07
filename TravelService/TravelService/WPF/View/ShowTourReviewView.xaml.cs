using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelService.Applications.Utils;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for ShowTourReviewView.xaml
    /// </summary>
    public partial class ShowTourReviewView : Page, INotifyPropertyChanged
    {
        
        public ShowTourReviewView(Guest selectGuest,TourReview selectedTourReview,NavigationService navigationService)
        {
            InitializeComponent();  
           this.DataContext= new ShowTourReviewsViewModel(selectGuest, selectedTourReview,navigationService,this); 
        
            



        }
        public event PropertyChangedEventHandler? PropertyChanged;

    }
}
