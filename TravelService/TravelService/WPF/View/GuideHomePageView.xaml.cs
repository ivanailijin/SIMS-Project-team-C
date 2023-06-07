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
using TravelService.Domain.Model;
using TravelService.Repository;


using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for SecondGuestView.xaml
    /// </summary>
    public partial class GuideHomePageView : Window,INotifyPropertyChanged
    {

     

        public GuideHomePageView(Guide guide)
        {
           
            InitializeComponent();
            this.DataContext = new GuideHomePageViewModel(this.Main.NavigationService, guide, this);
           
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        
    }
}