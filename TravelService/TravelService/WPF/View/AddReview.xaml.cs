using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for AddReview.xaml
    /// </summary>
    public partial class AddReview : Window, INotifyPropertyChanged
    {
        public AddReview()
        {
            InitializeComponent();
            AddReviewViewModel addReviewViewModel = new AddReviewViewModel();
            DataContext= addReviewViewModel;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
