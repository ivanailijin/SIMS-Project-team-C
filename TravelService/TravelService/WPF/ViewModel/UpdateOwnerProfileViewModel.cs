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

namespace TravelService.WPF.ViewModel
{
    public class UpdateOwnerProfileViewModel : ViewModelBase
    {
        public Owner Owner { get; set; }
        public Action CloseAction { get; set; }
        public RelayCommand ConfirmCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        public OwnerService ownerService;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public UpdateOwnerProfileViewModel(Owner owner)
        {
            this.Owner = owner;
            ownerService = new OwnerService(Injector.CreateInstance<IOwnerRepository>());
            InitializeCommands();
        }
        private void InitializeCommands()
        {
            ConfirmCommand = new RelayCommand(Execute_ConfirmCommand, CanExecute_Command);
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
        }
        private void Execute_ConfirmCommand(object obj)
        {
            ownerService.Update(Owner);
            CloseAction();
        }
        private void Execute_CancelCommand(object obj)
        {
            CloseAction();
        }
        private bool CanExecute_Command(object arg)
        {
            return true;
        }
    }
}
