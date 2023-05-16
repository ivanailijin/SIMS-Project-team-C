using System;
using System.ComponentModel;
using System.Windows;
using TravelService.Domain.Model;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for AddTour.xaml
    /// </summary>
    public partial class AddTourView : Window, INotifyPropertyChanged
    {
        public AddTourView(Guide guide, bool visibility)
        {
            InitializeComponent();
            AddTourViewModel addTourViewModel = new AddTourViewModel(guide, visibility);
            
            DataContext = addTourViewModel;
            if (addTourViewModel.CloseAction == null)
                addTourViewModel.CloseAction = new Action(this.Close);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
