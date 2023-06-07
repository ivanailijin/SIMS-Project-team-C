using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.View;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class OwnerProfileViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public Owner Owner { get; set; }
        public Action CloseAction { get; set; }
        public OwnerService ownerService { get; set; }
        public OwnerProfileView OwnerProfileView { get; set; }
        public RelayCommand UpdateDataCommand { get; set; }
        public RelayCommand SaveDataCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        public AccommodationService accommodationService;

        private bool _isSuperOwner;
        public bool IsSuperOwner
        {
            get => _isSuperOwner;
            set
            {
                if (value != _isSuperOwner)
                {
                    _isSuperOwner = value;
                    OnPropertyChanged(nameof(IsSuperOwner));
                }
            }
        }

        private int _numberOfAccommodations;
        public int NumberOfAccommodations
        {
            get => _numberOfAccommodations;
            set
            {
                if (value != _numberOfAccommodations)
                {
                    _numberOfAccommodations = value;
                    OnPropertyChanged(nameof(IsSuperOwner));
                }
            }
        }
        private bool _isFormEnabled;
        public bool IsFormEnabled
        {
            get { return _isFormEnabled; }
            set
            {
                if (_isFormEnabled != value)
                {
                    _isFormEnabled = value;
                    OnPropertyChanged(nameof(IsFormEnabled));
                }
            }
        }
        private bool _isSaveEnabled;
        public bool IsSaveEnabled
        {
            get { return _isSaveEnabled; }
            set
            {
                if (_isSaveEnabled != value)
                {
                    _isSaveEnabled = value;
                    OnPropertyChanged(nameof(IsSaveEnabled));
                }
            }
        }
        private bool _isChangeEnabled;
        public bool IsChangeEnabled
        {
            get { return _isChangeEnabled; }
            set
            {
                if (_isChangeEnabled != value)
                {
                    _isChangeEnabled = value;
                    OnPropertyChanged(nameof(IsChangeEnabled));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public OwnerProfileViewModel(Owner owner, OwnerProfileView ownerProfileView)
        {
            this.Owner = owner;
            ownerService = new OwnerService(Injector.CreateInstance<IOwnerRepository>());
            accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>());
            NumberOfAccommodations = accommodationService.GetNumberOfAccommodations(Owner.Id);
            IsSuperOwner = owner.SuperOwner;
            InitializeCommands();
            IsFormEnabled = false;
            IsSaveEnabled = IsFormEnabled;
            IsChangeEnabled = !IsFormEnabled;
            OwnerProfileView = ownerProfileView;
        }
        private void InitializeCommands()
        {
            UpdateDataCommand = new RelayCommand(Execute_UpdateDataCommand, CanExecute_Command);
            SaveDataCommand = new RelayCommand(Execute_SaveDataCommand, CanExecute_Command);
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
        }
        private void Execute_UpdateDataCommand(object obj)
        {
            IsFormEnabled = true;
            IsSaveEnabled = IsFormEnabled;
            IsChangeEnabled = !IsFormEnabled;
        }
        private void Execute_SaveDataCommand(object obj)
        {
            ownerService.Update(Owner);
            IsFormEnabled = false;
            IsSaveEnabled = IsFormEnabled;
            IsChangeEnabled = !IsFormEnabled;
        }
        private void Execute_CancelCommand(object obj)
        {
            OwnerProfileView.GoBack();
        }
        private bool CanExecute_Command(object arg)
        {
            return true;
        }
    }
}
