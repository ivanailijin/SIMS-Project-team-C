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

namespace TravelService.View
{
    /// <summary>
    /// Interaction logic for GuestRatingView.xaml
    /// </summary>
    public partial class GuestRatingView : Window, INotifyPropertyChanged
    {
        private int ReservationId;

        private int _cleannes;
        public int Cleanness
        {
            get => _cleannes;
            set
            {
                if (value != _cleannes)
                {
                    _cleannes = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _rulesFollowing;
        public int RulesFollowing
        {
            get => _rulesFollowing;
            set
            {
                if (value != _rulesFollowing)
                {
                    _rulesFollowing = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _noiseLevel;
        public int NoiseLevel
        {
            get => _noiseLevel;
            set
            {
                if (value != _noiseLevel)
                {
                    _noiseLevel = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _communication;
        public int Communication
        {
            get => _communication;
            set
            {
                if (value != _communication)
                {
                    _communication = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _propertyRespect;
        public int PropertyRespect
        {
            get => _propertyRespect;
            set
            {
                if (value != _propertyRespect)
                {
                    _propertyRespect = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _comment;
        public int Comment
        {
            get => _comment;
            set
            {
                if (value != _comment)
                {
                    _comment = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public GuestRatingView(int reservationId)
        {
            InitializeComponent();
            ReservationId = reservationId;
        }
    }
}
