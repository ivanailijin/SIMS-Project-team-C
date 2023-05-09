using System;
using System.ComponentModel;
using System.Windows;
using TravelService.Domain.Model;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for EnterCheckPoint.xaml
    /// </summary>
    public partial class EnterCheckPointView : Window,INotifyPropertyChanged
    {
        
        public EnterCheckPointView(int Id)
        {

            InitializeComponent();
            EnterCheckPointViewModel enterCheckPointViewModel = new EnterCheckPointViewModel(Id);
            DataContext = enterCheckPointViewModel;
            if (enterCheckPointViewModel.CloseAction == null)
                enterCheckPointViewModel.CloseAction = new Action(this.Close);
        }
        public event PropertyChangedEventHandler? PropertyChanged;

    }
}
