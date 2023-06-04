using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class SelectedForumViewModel : ViewModelBase
    {
        public Guest1 Guest1 { get; set; }
        public SelectedForumView SelectedForumView { get; set; }
        public SelectedForumViewModel(SelectedForumView selectedForumView, Guest1 guest)
        {
            Guest1 = guest;
            SelectedForumView = selectedForumView;
        }
    }
}
