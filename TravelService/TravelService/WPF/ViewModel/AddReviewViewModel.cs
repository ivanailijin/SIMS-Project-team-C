using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;

namespace TravelService.WPF.ViewModel
{
    public class AddReviewViewModel : ViewModelBase
    {
        public Guest2 Guest2 { get; set; }
        public AddReviewViewModel(Tour selectedTour, Guest2 guest2) 
        {
            Guest2 = guest2;
        }
    }
}
