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
using TravelService.Repository;
using TravelService.Domain.Model;
using TravelService.Validation;
using System.Text.RegularExpressions;
using System.Globalization;
using Microsoft.Win32;
using System.IO;
using TravelService.Domain.Model;
using TravelService.WPF.ViewModel;
using TravelService.WPF.Services;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for AddAccommodation.xaml
    /// </summary>
    public partial class AddAccommodation : Page, INotifyPropertyChanged, INavigationInterface
    {
        public AddAccommodation(Owner owner, Location location)
        {
            InitializeComponent();
            AddAccommodationViewModel addAccommodationViewModel = new AddAccommodationViewModel(owner, location, this);
            DataContext = addAccommodationViewModel;
        }
        public void GoBack()
        {
            NavigationService?.GoBack();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
