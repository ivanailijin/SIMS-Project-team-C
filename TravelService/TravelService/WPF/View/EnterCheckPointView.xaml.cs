using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using TravelService.Domain.Model;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for EnterCheckPoint.xaml
    /// </summary>
    public partial class EnterCheckPointView : Page,INotifyPropertyChanged
    {
        
        public EnterCheckPointView(int Id,NavigationService navigationService)
        {

            InitializeComponent();
            this.DataContext = new EnterCheckPointViewModel(this, Id, navigationService);
          
        }
        public event PropertyChangedEventHandler? PropertyChanged;

    }
}
