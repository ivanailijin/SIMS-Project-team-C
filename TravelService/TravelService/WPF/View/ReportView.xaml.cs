using System;
using System.Collections.Generic;
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
using TravelService.Domain.Model;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for ReportView.xaml
    /// </summary>
    public partial class ReportView : Page
    {
        
        public ReportView(TourReview selectedTourReview,Guest selectedguest,NavigationService navigationService)
        {
            InitializeComponent();
           this.DataContext = new ReportViewModel(selectedTourReview, selectedguest, navigationService,this);
          
            

        }
    }
}
