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
    public class OwnerProfileViewModel : ViewModelBase
    {
        public Owner Owner { get; set; }
        public Action CloseAction { get; set; }

        public OwnerProfileView OwnerProfileView { get; set; }
        public RelayCommand UpdateDataCommand { get; set; }
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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public OwnerProfileViewModel(Owner owner, OwnerProfileView ownerProfileView)
        {
            this.Owner = owner;
            accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>());
            NumberOfAccommodations = accommodationService.GetNumberOfAccommodations(Owner.Id);
            IsSuperOwner = owner.SuperOwner;
            InitializeCommands();
            OwnerProfileView = ownerProfileView;
        }
        private void InitializeCommands()
        {
            UpdateDataCommand = new RelayCommand(Execute_UpdateDataCommand, CanExecute_Command);
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
        }
        private void Execute_UpdateDataCommand(object obj)
        {
            UpdateOwnerProfileView updateOwnerProfileView = new UpdateOwnerProfileView(Owner);
            updateOwnerProfileView.Show();
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
