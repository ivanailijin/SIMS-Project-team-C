using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.WPF.Services;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class SelectedForumViewModel : ViewModelBase
    {
        public Guest1 Guest1 { get; set; }
        public SelectedForumView SelectedForumView { get; set; }
        public Forum SelectedForum { get; set; }

        private RelayCommand _previousPageCommand;
        public RelayCommand PreviousPageCommand
        {
            get => _previousPageCommand;
            set
            {
                if (value != _previousPageCommand)
                {
                    _previousPageCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        public SelectedForumViewModel(SelectedForumView selectedForumView, Guest1 guest, Forum selectedForum)
        {
            Guest1 = guest;
            SelectedForumView = selectedForumView;
            SelectedForum = selectedForum;

            PreviousPageCommand = new RelayCommand(Execute_PreviousPage, CanExecute_Command);
        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

        public void GoBack()
        {
            SelectedForumView.GoBack();
        }

        private void Execute_PreviousPage(object sender)
        {
            GoBack();
        }
    }
}
