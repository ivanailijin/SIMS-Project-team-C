using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.WPF.ViewModel
{
    public class AddLanguageViewModel : ViewModelBase
    {
        public int LanguageId;
        private readonly LanguageService _languageService;
        private readonly TourService _tourService;
        public List<Language> Languages { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public Action CloseAction { get; set; }

        private Language _selectedLanguage;

        public Language SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (value != _selectedLanguage)
                {
                    _selectedLanguage = value;
                    OnPropertyChanged();
                }
            }
        }
        public AddLanguageViewModel(int Id)
        {
            _tourService = new TourService(Injector.CreateInstance<ITourRepository>());
            _languageService = new LanguageService(Injector.CreateInstance<ILanguageRepository>());
            LanguageId = Id;

            Languages = _languageService.GetAll();
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
            SaveCommand = new RelayCommand(Execute_CreateCommand, CanExecute_Command);
        }


        private void Execute_CreateCommand(object obj)
        {
            if (SelectedLanguage == null)
            {
                MessageBox.Show("Please select language first.");
                return;
            }

            Language selectedLanguage = SelectedLanguage;
            selectedLanguage.Id = LanguageId;
            Language savedLanguage = _languageService.Save(SelectedLanguage);

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