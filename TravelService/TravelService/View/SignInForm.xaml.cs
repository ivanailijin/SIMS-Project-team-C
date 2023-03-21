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
using TravelService.Model;
using TravelService.Repository;

namespace TravelService.View
{
    /// <summary>
    /// Interaction logic for SignInForm.xaml
    /// </summary>
    public partial class SignInForm : Window
    {

        private readonly UserRepository _repository;
        private readonly Guest1Repository _guest1Repository;

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        public string _password;

        public string Password
        {
            get => _password;
            set
            {
                if (value != _password)
                {
                    _password = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool OwnerIsChecked { get; set; }
        public bool Guest2IsChecked { get; set; }
        public bool GuideIsChecked { get; set; }

        private bool _guest1IsChecked;

        public bool Guest1IsChecked
        {
            get { return _guest1IsChecked; }
            set
            {
                _guest1IsChecked = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SignInForm()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new UserRepository();
            _guest1Repository = new Guest1Repository();

            Password = txtPassword.Password;
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(txtPassword.Password))
            {
                User user = _repository.GetByUsername(Username);

                if (user != null)
                {
                    if (user.Password.Equals(txtPassword.Password))
                    {
                        if (OwnerIsChecked && user.UserType.Equals("Owner"))
                        {
                            //Owner owner = _ownerRepository.GetByUsername(Username);
                        }
                        else if (Guest1IsChecked && user.UserType.Equals("Guest1"))
                        {
                            Guest1 guest1 = _guest1Repository.GetByUsername(Username);
                            AccommodationView accommodationView = new AccommodationView(guest1);
                            accommodationView.Show();
                        }
                        else if (Guest2IsChecked && user.UserType.Equals("Guest2"))
                        {

                        }
                        else if (GuideIsChecked && user.UserType.Equals("Guide"))
                        {

                        }
                        else
                        {
                            MessageBox.Show("Wrong user type!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Wrong password!");
                    }
                }
                else
                {
                    MessageBox.Show("Wrong username!");
                }
            }
        }
    }
}
