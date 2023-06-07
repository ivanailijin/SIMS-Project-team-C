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
    /// Interaction logic for ComplexRequests.xaml
    /// </summary>
    public partial class ComplexRequests : Page
    {
        public ComplexRequests(Guide guide,TourRequest selectedREq,ComplexTourRequest selectedComplex,NavigationService navigationSerivce)
        {
            InitializeComponent();
            this.DataContext = new ComplexRequestsViewModel(guide,selectedREq, this,selectedComplex, navigationSerivce);
        }
    }
}
